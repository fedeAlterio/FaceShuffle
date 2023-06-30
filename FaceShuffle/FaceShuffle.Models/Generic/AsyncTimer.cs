#nullable enable

using System.Diagnostics;
using FaceShuffle.Models.Extensions;

namespace FaceShuffle.Models.Generic
{
    public delegate Task CancellableAsyncAction(CancellationToken cancellationToken);
    public class AsyncTimer
    {
        #region Fields

        readonly TimeSpan _period;
        readonly TimeSpan _dueTime;
        readonly Func<TimeSpan, CancellationToken, Task> _taskDelay;
        readonly Func<DateTime> _dateTimeNow;
        readonly CancellableAsyncAction _onTick;
        readonly TaskScheduler _taskScheduler;
        readonly object _locker = new();
        CancellationTokenSource? _timerStopCts;
        HandlerCancellationInfo? _handlerCancelCts;
        Task _currentOnTickHandler = Task.CompletedTask;

        bool _isStarted;

        #endregion

        #region Delegates and events

        public event EventHandler<Exception>? OnException;

        #endregion

        #region Properties

        public bool IsStarted
        {
            get
            {
                lock (_locker)
                {
                    return _isStarted;
                }
            }
            private set
            {
                _isStarted = value;
                if (!value)
                    DisposeHandlerCtsIfNeeded_Unlocked();
            }
        }

        #endregion

        #region Public methods

        public void Start()
        {
            CancellationToken timerStopToken;
            lock (_locker)
            {
                if (_isStarted)
                    return;

                // We setup a cancellation token that stops the timer loop (but not the handlers)
                Debug.Assert(_timerStopCts is null);
                _timerStopCts = new();
                timerStopToken = _timerStopCts.Token;

                IsStarted = true;
            }

            _ = StartNewTimerLoop(timerStopToken);
        }

        public void Stop()
        {
            lock (_locker)
            {
                StopPrivate();
            }
        }

        public Task StopAndCancelCurrentAsync(bool removeScheduledExtraTick = true) => CancelPrivate(true, removeScheduledExtraTick);
        public Task CancelCurrentAsync(bool removeScheduledExtraTick = false) => CancelPrivate(false,      removeScheduledExtraTick);

        public Task WaitForCurrentHandlerAsync()
        {
            lock (_locker)
            {
                return _currentOnTickHandler;
            }
        }

        public async Task ScheduleAndWaitExtraTickAsync()
        {
            // Step 1, wee see if there is an handler running
            Task currentOnTickHandler;
            CancellationToken handlerCancellationToken;
            HandlerCancellationInfo handlerCancellationInfo;
            lock (_locker)
            {
                _handlerCancelCts ??= new();
                handlerCancellationInfo = _handlerCancelCts;
                handlerCancellationToken = handlerCancellationInfo.CancellationTokenSource.Token;
                currentOnTickHandler = _currentOnTickHandler;
            }

            // We wait for the handler to complete. If it is already completed this is a nop.
            await currentOnTickHandler.ConfigureAwait(false);

            lock (_locker)
            {
                // If something canceled the handler we were waiting to complete, check if it also canceled the eventual extra tick.
                // If thats the case, we just early return
                if (handlerCancellationToken.IsCancellationRequested && handlerCancellationInfo.RemoveScheduledExtraTick)
                    return;

                // If someone just started another tick while we were locked we use that as the extra tick.
                // Otherwise we are the first, and we schedule an extra tick
                currentOnTickHandler = _currentOnTickHandler != currentOnTickHandler
                    ? _currentOnTickHandler
                    : InvokeOnTickIfNotRunning();
            }

            // We wait for the extra tick to finish
            await currentOnTickHandler.ConfigureAwait(false);
        }

        #endregion

        #region Private methods

        Task StartNewTimerLoop(CancellationToken timerStopToken)
        {
            // The timer loop is running on the threadpool. So that in UI frameworks we avoid masrshalling back and forth the UI thread for nothing
            return Task.Run(() => TimerLoop(timerStopToken), timerStopToken);
        }

        async Task TimerLoop(CancellationToken timerStopToken)
        {
            // If there is a due time we just wait for it before we start the timer loop
            if (_dueTime > TimeSpan.Zero)
            {
                await _taskDelay(_dueTime, timerStopToken).ConfigureAwait(false);
            }

            // We extract the startDate. This is used to correct the eventual dellay introduced by the asynchronous handler
            var startDateUtc = _dateTimeNow();

            while (!timerStopToken.IsCancellationRequested)
            {
                lock (_locker)
                {
                    // Before even trying to see if we can schedule another tick we ensure that someone did not stop the timer meanwhile
                    timerStopToken.ThrowIfCancellationRequested();
                    _ = InvokeOnTickIfNotRunning();
                }

                var now = _dateTimeNow();
                var expectedTicks = (now - startDateUtc).Ticks / _period.Ticks;
                var targetDate = startDateUtc + TimeSpan.FromTicks((expectedTicks + 1) * _period.Ticks);

                var delay = targetDate - now;
                if (delay > TimeSpan.Zero)
                {
                    await _taskDelay(delay, timerStopToken).ConfigureAwait(false);
                }
            }
        }

        void StopPrivate()
        {
            if (!_isStarted)
                return;

            Debug.Assert(_timerStopCts is not null);
            var timerStopCts = _timerStopCts!;
            _timerStopCts = null;

            // We cancel the cancellation token that stops the timer loop (but not the handlers)
            timerStopCts.Cancel();
            timerStopCts.Dispose();

            IsStarted = false;
        }

        Task InvokeOnTickIfNotRunning()
        {
            // If there is already a tick running, we just return
            if (!_currentOnTickHandler.IsCompleted)
                return _currentOnTickHandler;

            // We setup a cancellation token for the handler.
            _handlerCancelCts ??= new();
            var handlerCancellationToken = _handlerCancelCts.CancellationTokenSource.Token;

            // We asynchronously invoke the tick on the caller context
            _currentOnTickHandler = InvokeOnTickHandler(handlerCancellationToken);

            // Whenever the tick completes, we check if we can dispose the handler cancellation token
            _currentOnTickHandler.ContinueWith(_ => DisposeHandlerCtsIfNeeded(), CancellationToken.None);

            return _currentOnTickHandler;
        }

        Task InvokeOnTickHandler(CancellationToken handlerCancellationToken)
        {
            // We asynchronously invoke the tick on the caller context
            return Task.Factory.StartNew(() => TryInvokeOnTickHandler(handlerCancellationToken),
                                         handlerCancellationToken, TaskCreationOptions.DenyChildAttach, _taskScheduler).Unwrap();
        }

        async Task TryInvokeOnTickHandler(CancellationToken cancellationToken)
        {
            // Here we are on the caller context. we invoke the handler and we catch eventual exceptions

            try
            {
                await _onTick.Invoke(cancellationToken);
            }
            catch (Exception ex)
            {
                OnException?.Invoke(this, ex);
            }
        }

        void DisposeHandlerCtsIfNeeded()
        {
            lock (_locker)
            {
                DisposeHandlerCtsIfNeeded_Unlocked();
            }
        }

        void DisposeHandlerCtsIfNeeded_Unlocked()
        {
            // We can dispose the handler cancellation token source only if the timer is stopped and we are not executing a tick
            if (_isStarted || !_currentOnTickHandler.IsCompleted)
            {
                return;
            }

            DisposeHandlerCts();
        }

        void DisposeHandlerCts()
        {
            var handlerCancelCts = _handlerCancelCts;
            if (handlerCancelCts is null)
                return;

            _handlerCancelCts = null;
            handlerCancelCts.Dispose();
        }

        async Task CancelPrivate(bool shouldAlsoStop, bool removeScheduledExtraTick)
        {
            Task currentOnTickHandler;

            lock (_locker)
            {
                if (shouldAlsoStop)
                    StopPrivate();

                currentOnTickHandler = _currentOnTickHandler;
                if (_handlerCancelCts is not null)
                {
                    _handlerCancelCts.CancellationTokenSource.Cancel();
                    _handlerCancelCts.RemoveScheduledExtraTick = removeScheduledExtraTick;
                    DisposeHandlerCts();
                }

                if (currentOnTickHandler.IsCompleted)
                    return;
            }

            await currentOnTickHandler.SuppressExceptions()
                                      .ConfigureAwait(false);
        }

        /// <summary>
        /// If a SynchronizationContext is set for the current thread (for example we are in the UI thread of Wpf), we use that to schedule ticks
        /// handlers.
        /// Otherwise we use the current TaskScheduler (that defaults to the ThreadPool).
        /// </summary>
        static TaskScheduler GetCurrentTaskScheduler() => SynchronizationContext.Current switch
        {
            null => TaskScheduler.Current,
            _ => TaskScheduler.FromCurrentSynchronizationContext()
        };

        #endregion

        #region Constructor
        /// <summary>
        /// This constructor is here only for testing purpouses.
        /// </summary>
        /// <param name="period">Period of the timer</param>
        /// <param name="dueTime">Time to wait before invoking the first tick</param>
        /// <param name="onTick">Handler to invoke on timer tick</param>
        /// <param name="taskScheduler">TaskScheduler on which execute the handler</param>
        /// <param name="taskDelay">Function to fake <see cref="Task.Delay(TimeSpan, CancellationToken)"/></param>
        /// <param name="dateTimeNow">Function to fake <see cref="DateTime.Now"/></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        internal AsyncTimer(TimeSpan period,
                                     TimeSpan dueTime,
                                     CancellableAsyncAction onTick,
                                     TaskScheduler taskScheduler,
                                     Func<TimeSpan, CancellationToken, Task> taskDelay,
                                     Func<DateTime> dateTimeNow)
        {
            if (period <= TimeSpan.Zero)
                throw new ArgumentException("Value should be greater than 0", nameof(period));

            if (dueTime < TimeSpan.Zero)
                throw new ArgumentException("Value should be greater of equals to 0", nameof(dueTime));

            if (onTick is null)
                throw new ArgumentNullException(nameof(onTick));

            if (taskScheduler is null)
                throw new ArgumentNullException(nameof(taskScheduler));

            _period = period;
            _dueTime = dueTime;
            _taskDelay = taskDelay;
            _dateTimeNow = dateTimeNow;
            _onTick = onTick;
            _taskScheduler = taskScheduler;
        }

        /// <param name="period">Period of the timer</param>
        /// <param name="dueTime">Time to wait before invoking the first tick</param>
        /// <param name="onTick">Handler to invoke on timer tick</param>
        /// <param name="taskScheduler">TaskScheduler on which execute the handler</param>
        public AsyncTimer(TimeSpan period, TimeSpan dueTime, CancellableAsyncAction onTick, TaskScheduler taskScheduler)
            : this(period, dueTime, onTick, taskScheduler, Task.Delay, static () => DateTime.UtcNow)
        {
        }
   

        /// <param name="period">Period of the timer</param>
        /// <param name="dueTime">Time to wait before invoking the first tick</param>
        /// <param name="onTick">Handler to invoke on timer tick</param>
        public AsyncTimer(TimeSpan period, TimeSpan dueTime, CancellableAsyncAction onTick)
            : this(period, dueTime, onTick, GetCurrentTaskScheduler())
        {
            
        }

        /// <param name="period">Period of the timer</param>
        /// <param name="onTick">Handler to invoke on timer tick</param>
        public AsyncTimer(TimeSpan period, CancellableAsyncAction onTick) 
            : this(period, period, onTick)
        {
        }

        
        /// <param name="period">Period of the timer</param>
        /// <param name="onTick">Handler to invoke on timer tick</param>
        public AsyncTimer(int period, CancellableAsyncAction onTick) 
            : this(TimeSpan.FromMilliseconds(period), TimeSpan.FromMilliseconds(period), onTick)
        {
        }


        /// <param name="period">Period of the timer</param>
        /// <param name="dueTime">Time to wait before invoking the first tick</param>
        /// <param name="onTick">Handler to invoke on timer tick</param>
        public AsyncTimer(int period, int dueTime, CancellableAsyncAction onTick) 
            : this(TimeSpan.FromMilliseconds(period), TimeSpan.FromMilliseconds(dueTime), onTick)
        {
        }

        #endregion

        #region Private Classes

        class HandlerCancellationInfo : IDisposable
        {
            #region Properties

            public CancellationTokenSource CancellationTokenSource { get; } = new();
            public bool RemoveScheduledExtraTick { get; set; }

            #endregion

            #region Constructor

            public void Dispose() => CancellationTokenSource.Dispose();

            #endregion
        }

        #endregion
    }
}