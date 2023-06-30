using System.Runtime.CompilerServices;
using FaceShuffle.Models.Generic;

namespace FaceShuffle.Models.Extensions
{
    public static class AsyncHelpers
    {
        /// <summary>
        /// Use this method to suppress warnings when you don't want to wait for the task to finish
        /// This method also suppress any exception throwed by the Task
        /// </summary>
        /// <param name="this">The task to fire and forget</param>
        /// <exception cref="ArgumentNullException"><param name="this">is null</param></exception>
        public static void FireAndForget(this Task @this)
        {
            if (@this is null)
                throw new ArgumentNullException(nameof(@this));

            _ = @this.SuppressExceptions();
        }


        /// <summary>
        /// Use this method if you want to wait the task only until the cancellation token is raised
        /// </summary>
        /// <param name="this">The initial task</param>
        /// <param name="cancellationToken">The cancellation token that stops the waiting</param>
        /// <returns>A Task that will be either canceled when the cancellation token is raised or completed when the initial task completes</returns>
        /// <exception cref="ArgumentNullException"><param name="this">is null</param></exception>
        /// <exception cref="OperationCanceledException">When the cancellationToken is canceled</exception>
        public static Task ThrowOnCancellation(this Task @this, CancellationToken cancellationToken)
        {
            if (@this is null)
                throw new ArgumentNullException(nameof(@this));

            if (!cancellationToken.CanBeCanceled)
                return @this;

            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled(cancellationToken);

            return Core();

            async Task Core()
            {
                var tcs = new TaskCompletionSource<object>();
#if NETCOREAPP3_1_OR_GREATER
                await 
#endif
                using var _ = cancellationToken.Register(() => tcs.TrySetCanceled());

                cancellationToken.ThrowIfCancellationRequested();
                await Task.WhenAny(tcs.Task, @this)
                          .ConfigureAwait(false);

                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        /// <summary>
        /// Use this method if you want to wait the task only until the cancellation token is raised
        /// </summary>
        /// <param name="this">The initial task</param>
        /// <param name="cancellationToken">The cancellation token that stops the waiting</param>
        /// <returns>A Task that will be either canceled when the cancellation token is raised or completed when the initial task completes</returns>
        /// <exception cref="ArgumentNullException"><param name="this">is null</param></exception>
        /// <exception cref="OperationCanceledException">When the cancellationToken is canceled</exception>
        public static Task<T> ThrowOnCancellation<T>(this Task<T> @this, CancellationToken cancellationToken)
        {
            if (@this is null)
                throw new ArgumentNullException(nameof(@this));

            if (!cancellationToken.CanBeCanceled)
                return @this;

            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled<T>(cancellationToken);

            return Core();

            async Task<T> Core()
            {
                var tcs = new TaskCompletionSource<object>();
#if NETCOREAPP3_1_OR_GREATER
                await 
#endif
                using var _ = cancellationToken.Register(() => tcs.TrySetCanceled());

                cancellationToken.ThrowIfCancellationRequested();
                await Task.WhenAny(tcs.Task, @this)
                          .ConfigureAwait(false);

                cancellationToken.ThrowIfCancellationRequested();
                return @this.Result;
            }
        }

        /// <summary>
        /// Use this method if you want to wait the task only until timeout
        /// </summary>
        /// <param name="this">The initial task</param>
        /// <param name="timeoutPeriod">The timoeut period</param>
        /// <returns>A Task that will be either canceled when the timeout is expired or completed when the initial task completes</returns>
        /// <exception cref="ArgumentNullException"><param name="this">is null</param></exception>
        /// <exception cref="OperationCanceledException">Timeout has expired</exception>
        public static Task<T> ThrowOnTimeout<T>(this Task<T> @this, TimeSpan timeoutPeriod)
        {
            if (@this is null)
                throw new ArgumentNullException(nameof(@this));

            if (timeoutPeriod < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeoutPeriod));

            return Core();

            async Task<T> Core()
            {
                var valueOptional = await @this.WithTimeout(timeoutPeriod)
                                               .ConfigureAwait(false);

                if (!valueOptional.TryGetValue(out var value))
                    throw new OperationCanceledException();

                return value;
            }
        }


        /// <summary>
        /// Use this method if you want to wait the task only until timeout
        /// </summary>
        /// <param name="this">The initial task</param>
        /// <param name="timeoutPeriod">The timoeut period</param>
        /// <returns>A Task that will be either canceled when the timeout is expired or completed when the initial task completes</returns>
        /// <exception cref="ArgumentNullException"><param name="this">is null</param></exception>
        /// <exception cref="OperationCanceledException">Timeout has expired</exception>
        public static Task ThrowOnTimeout(this Task @this, TimeSpan timeoutPeriod)
        {
            if (@this is null)
                throw new ArgumentNullException(nameof(@this));

            if (timeoutPeriod < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeoutPeriod));

            return Core();

            async Task Core()
            {
                var isTimedOut = await @this.WithTimeout(timeoutPeriod)
                                            .ConfigureAwait(false);

                if (isTimedOut)
                    throw new OperationCanceledException();
            }
        }


        /// <summary>
        /// Use this method if you want to wait the task only until timeout
        /// This method does not throw an exception when timeout is expired
        /// </summary>
        /// <param name="this">The initial task</param>
        /// <param name="timeoutPeriod">The timoeut period</param>
        /// <returns>A Task that will wrap a result of true when the timeout is expired or false when the initial task completes</returns>
        /// <exception cref="ArgumentNullException"><param name="this">is null</param></exception>
        public static Task<bool> WithTimeout(this Task @this, TimeSpan timeoutPeriod)
        {
            if (@this is null)
                throw new ArgumentNullException(nameof(@this));

            if (timeoutPeriod == Timeout.InfiniteTimeSpan)
                return @this.ContinueWith(static _ => false, TaskContinuationOptions.ExecuteSynchronously);

            if (timeoutPeriod < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeoutPeriod));

            return Core();

            async Task<bool> Core()
            {
                using var delayCts = new CancellationTokenSource();
                var timeoutTask = Task.Delay(timeoutPeriod, delayCts.Token);
                var completedTask = await Task.WhenAny(@this, timeoutTask)
                                              .ConfigureAwait(false);

                var isTimedOut = completedTask == timeoutTask;
                if (isTimedOut)
                    return true;

                delayCts.Cancel();
                return false;
            }
        }


        /// <summary>
        /// Use this method if you want to wait the task only until timeout
        /// This method does not throw an exception when timeout is expired
        /// </summary>
        /// <param name="this">The initial task</param>
        /// <param name="timeoutPeriod">The timoeut period</param>
        /// <returns>A Task that contains as Result an optional. This optional will be empty if the timeout is expired, or it will contains the original Task Result otherwise</returns>
        /// <exception cref="ArgumentNullException"><param name="this">is null</param></exception>
        public static Task<Optional<T>> WithTimeout<T>(this Task<T> @this, TimeSpan timeoutPeriod)
        {
            if (@this is null)
                throw new ArgumentNullException(nameof(@this));

            if (timeoutPeriod == Timeout.InfiniteTimeSpan)
                return @this.ContinueWith(static x => new Optional<T>(x.Result), TaskContinuationOptions.ExecuteSynchronously);

            if (timeoutPeriod < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeoutPeriod));

            return Core();

            async Task<Optional<T>> Core()
            {
                using var delayCts = new CancellationTokenSource();
                var timeoutTask = Task.Delay(timeoutPeriod, delayCts.Token);
                var completedTask = await Task.WhenAny(@this, timeoutTask)
                                              .ConfigureAwait(false);

                var isTimedOut = completedTask == timeoutTask;
                if (isTimedOut)
                    return default;

                delayCts.Cancel();
                return new(@this.Result);
            }
        }


        /// <summary>
        /// <inheritdoc cref="Task.WhenAll(IEnumerable{Task})"/>
        /// </summary>
        /// <exception cref="ArgumentNullException"><param name="this">is null</param></exception>
        public static Task WhenAll(this IEnumerable<Task> @this)
        {
            if (@this is null)
                throw new ArgumentNullException(nameof(@this));

            return Task.WhenAll(@this);
        }

        /// <summary>
        /// <inheritdoc cref="Task.WhenAll{T}(IEnumerable{Task{T}})"/>
        /// <br></br>
        /// This methods throws an <see cref="OperationCanceledException"/> when cancellation token is canceled
        /// </summary>
        /// <exception cref="ArgumentNullException"><param name="this">is null</param></exception>
        public static Task WhenAll(this IEnumerable<Task> @this, CancellationToken cancellationToken)
        {
            if (@this is null)
                throw new ArgumentNullException(nameof(@this));

            return Task.WhenAll(@this).ThrowOnCancellation(cancellationToken);
        }


        /// <summary>
        /// <inheritdoc cref="Task.WhenAll(IEnumerable{Task})"/>
        /// </summary>
        /// <exception cref="ArgumentNullException"><param name="this">is null</param></exception>
        public static Task<T[]> WhenAll<T>(this IEnumerable<Task<T>> @this)
        {
            if (@this is null)
                throw new ArgumentNullException(nameof(@this));

            return Task.WhenAll(@this);
        }


        /// <summary>
        /// <inheritdoc cref="Task.WhenAll(IEnumerable{Task})"/>
        /// <br></br>
        /// /// This methods throws an <see cref="OperationCanceledException"/> when cancellation token is canceled
        /// </summary>
        /// <exception cref="ArgumentNullException"><param name="this">is null</param></exception>
        public static Task<T[]> WhenAll<T>(this IEnumerable<Task<T>> @this, CancellationToken cancellationToken)
        {
            if (@this is null)
                throw new ArgumentNullException(nameof(@this));

            return Task.WhenAll(@this).ThrowOnCancellation(cancellationToken);
        }

         /// <summary>
        /// <inheritdoc cref="Task.WhenAny(IEnumerable{Task})"/>
        /// </summary>
        /// <exception cref="ArgumentNullException"><param name="this">is null</param></exception>
        public static Task WhenAny(this IEnumerable<Task> @this)
        {
            if (@this is null)
                throw new ArgumentNullException(nameof(@this));

            return Task.WhenAny(@this);
        }


        /// <summary>
        /// <inheritdoc cref="Task.WhenAny{T}(IEnumerable{Task{T}})"/>z
        /// <br></br>
        /// This methods throws an <see cref="OperationCanceledException"/> when cancellation token is canceled
        /// </summary>
        /// <exception cref="ArgumentNullException"><param name="this">is null</param></exception>
        public static Task WhenAny(this IEnumerable<Task> @this, CancellationToken cancellationToken)
        {
            if (@this is null)
                throw new ArgumentNullException(nameof(@this));

            return Task.WhenAny(@this).ThrowOnCancellation(cancellationToken);
        }


        /// <summary>
        /// <inheritdoc cref="Task.WhenAny(IEnumerable{Task})"/>
        /// </summary>
        /// <exception cref="ArgumentNullException"><param name="this">is null</param></exception>
        public static Task<Task<T>> WhenAny<T>(this IEnumerable<Task<T>> @this)
        {
            if (@this is null)
                throw new ArgumentNullException(nameof(@this));

            return Task.WhenAny(@this);
        }


        #region TupleAwaiters

        #region NonGenericTask

        
        /// <summary>
        /// This method allows to call await over a tuple of tasks.
        /// It's equivalent to await a <see cref="Task.WhenAll(Task[])"/> 
        /// </summary>
        public static TaskAwaiter GetAwaiter(this (Task, Task) tasks)
        {
            return Core().GetAwaiter();

            async Task Core()
            {
                var (task1, task2) = tasks;
                await Task.WhenAll(task1, task2)
                          .ConfigureAwait(false);
            }
        }


        /// <summary>
        /// This method allows to call await over a tuple of tasks.
        /// It's equivalent to await a <see cref="Task.WhenAll(Task[])"/> 
        /// </summary>
        public static TaskAwaiter GetAwaiter(this (Task, Task, Task) tasks)
        {
            return Core().GetAwaiter();

            async Task Core()
            {
                var (task1, task2, task3) = tasks;
                await Task.WhenAll(task1, task2, task3)
                          .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// This method allows to call await over a tuple of tasks.
        /// It's equivalent to await a <see cref="Task.WhenAll(Task[])"/> 
        /// </summary>
        public static TaskAwaiter GetAwaiter(this (Task, Task, Task, Task) tasks)
        {
            return Core().GetAwaiter();

            async Task Core()
            {
                var (task1, task2, task3, task4) = tasks;
                await Task.WhenAll(task1, task2, task3, task4)
                          .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// This method allows to call await over a tuple of tasks.
        /// It's equivalent to await a <see cref="Task.WhenAll(Task[])"/> 
        /// </summary>
        public static TaskAwaiter GetAwaiter(this (Task, Task, Task, Task, Task) tasks)
        {
            return Core().GetAwaiter();

            async Task Core()
            {
                var (task1, task2, task3, task4, task5) = tasks;
                await Task.WhenAll(task1, task2, task3, task4, task5)
                          .ConfigureAwait(false);
            }
        }


        #endregion

        #region GenericTask

        /// <summary>
        /// This method allows to call await over a tuple of tasks.
        /// It's equivalent to await a <see cref="Task.WhenAll(Task[])"/> 
        /// <br></br>
        /// The result of the await will be a tuple containing the results of the tasks
        /// </summary>
        public static TaskAwaiter<(T1 result1, T2 result2)> GetAwaiter<T1, T2>(this (Task<T1> task1, Task<T2> task2) tasks)
        {
            return Core().GetAwaiter();

            async Task<(T1 result1, T2 result2)> Core()
            {
                var (task1, task2) = tasks;
                await Task.WhenAll(task1, task2)
                          .ConfigureAwait(false);

                return (task1.Result, task2.Result);
            }
        }


        /// <summary>
        /// <inheritdoc cref="GetAwaiter{T1,T2}"/>
        /// </summary>
        public static TaskAwaiter<(T1 result1, T2 result2, T3 result3)> GetAwaiter<T1, T2, T3>(this (Task<T1> task1, Task<T2> task2, Task<T3> task3) tasks)
        {
            return Core().GetAwaiter();

            async Task<(T1 result1, T2 result2, T3 result3)> Core()
            {
                var (task1, task2, task3) = tasks;
                await Task.WhenAll(task1, task2, task3)
                          .ConfigureAwait(false);

                return (task1.Result, task2.Result, task3.Result);
            }
        }

        /// <summary>
        /// <inheritdoc cref="GetAwaiter{T1,T2}"/>
        /// </summary>
        public static TaskAwaiter<(T1 result1, T2 result2, T3 result3, T4 result4)> GetAwaiter<T1, T2, T3, T4>(this (Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4) tasks)
        {
            return Core().GetAwaiter();

            async Task<(T1 result1, T2 result2, T3 result3, T4 result4)> Core()
            {
                var (task1, task2, task3, task4) = tasks;
                await Task.WhenAll(task1, task2, task3)
                          .ConfigureAwait(false);

                return (task1.Result, task2.Result, task3.Result, task4.Result);
            }
        }

        /// <summary>
        /// <inheritdoc cref="GetAwaiter{T1,T2}"/>
        /// </summary>
        public static TaskAwaiter<(T1 result1, T2 result2, T3 result3, T4 result4, T5 result5)> GetAwaiter<T1, T2, T3, T4, T5>(this (Task<T1> task1, Task<T2> task2, Task<T3> task3, Task<T4> task4, Task<T5> task5) tasks)
        {
            return Core().GetAwaiter();

            async Task<(T1 result1, T2 result2, T3 result3, T4 result4, T5 result5)> Core()
            {
                var (task1, task2, task3, task4, task5) = tasks;
                await Task.WhenAll(task1, task2, task3)
                          .ConfigureAwait(false);

                return (task1.Result, task2.Result, task3.Result, task4.Result, task5.Result);
            }
        }


        #endregion

        #endregion


        /// <summary>
        /// <inheritdoc cref="Task.WhenAny(IEnumerable{Task})"/>
        /// <br></br>
        /// /// This methods throws an <see cref="OperationCanceledException"/> when cancellation token is canceled
        /// </summary>
        /// <exception cref="ArgumentNullException"><param name="this">is null</param></exception>
        public static Task<Task<T>> WhenAny<T>(this IEnumerable<Task<T>> @this, CancellationToken cancellationToken)
        {
            if (@this is null)
                throw new ArgumentNullException(nameof(@this));

            return Task.WhenAny(@this).ThrowOnCancellation(cancellationToken);
        }


        
        /// <summary>
        /// This method suppress any exception contained inside the Task that is of type <see cref="TException"/>
        /// That means that all other type of exceptions are throwed as usual
        /// </summary>
        /// <param name="this">The orginal task</param>
        /// <returns>A task that completes whenever the original task completes, but without contain any exception</returns>
        /// <exception cref="ArgumentNullException"><param name="this">is null</param></exception>
        public static Task Suppress<TException>(this Task @this) where TException : Exception
        {
            if (@this is null)
                throw new ArgumentNullException(nameof(@this));

            return Core();

            async Task Core()
            {
                try
                {
                    await @this.ConfigureAwait(false);
                }
                catch(TException) 
                {
                    // Ignored
                }
            }
        }


        /// <summary>
        /// This method suppress any exception contained inside the Task
        /// </summary>
        /// <param name="this">The orginal task</param>
        /// <returns>A task that completes whenever the original task completes, but without contain any exception</returns>
        /// <exception cref="ArgumentNullException"><param name="this">is null</param></exception>
        public static Task SuppressExceptions(this Task @this)
        {
            return @this.Suppress<Exception>();
        }

        /// <summary>
        /// The object returned by this method can be seen as an async locker.
        /// Use it in combination with <see cref="ExecuteWithLock"/> to ensure that could not be 2 asynchronous operations
        /// executing the same block of code at the same time.
        /// </summary>
        /// <returns>A instance of <see cref="SemaphoreSlim"/> initialized with initial count = 1 and max count =1</returns>
        public static SemaphoreSlim NewAsyncLocker() => new(1, 1);


        /// <summary>
        /// Use this method to execute the <param name="action">action</param> after aquiring a slot of the semaphore
        /// The slot will be automatically released when the Task completes
        /// </summary>
        /// <param name="this">The Semaphore slim from which acquire the slot</param>
        /// <param name="action">The action to execute</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><param name="this">is null</param></exception>
        public static Task ExecuteWithLock(this SemaphoreSlim @this, Func<Task> action)
        {
            if (@this is null)
                throw new ArgumentNullException(nameof(@this));

            if (action is null)
                throw new ArgumentNullException(nameof(action));

            return Core();

            async Task Core()
            {
                await @this.WaitAsync();

                try
                {
                    await action();
                }
                finally
                {
                    @this.Release();
                }
            }
        }

        /// <summary>
        /// This method immediatly invokes <see cref="@this"/> and ensures
        /// that all the exceptions that it throws are wrapped inside the returned Task.
        /// For example if <see cref="@this"/> is a throwing synchronous delegate,
        /// the exception are not immediately throwed to the caller, but they are put inside the returned <see cref="Task.Exception"/>.
        /// It's only when the caller applies the await operator over the returned Task that the exception will be rethrowed to the caller
        /// </summary>
        /// <exception cref="ArgumentNullException"><param name="this">is null</param></exception>
        public static Task<T> InvokeAsync<T>(Func<Task<T>> @this)
        {
            if (@this is null)
                throw new ArgumentNullException(nameof(@this));

            return Core();

            async Task<T> Core()
            {
                return await @this().ConfigureAwait(false);
            }
        }


        /// <summary>
        /// <inheritdoc cref="InvokeAsync{T}(Func{Task{T}})"/>
        /// </summary>
        /// <exception cref="ArgumentNullException"><param name="this">is null</param></exception>
        public static Task InvokeAsync(Func<Task> @this)
        {
            if (@this is null)
                throw new ArgumentNullException(nameof(@this));

            return Core();

            async Task Core()
            {
                await @this().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// This method immediatly invokes <see cref="@this"/> and ensures
        /// that all the exceptions that it throws are wrapped inside the returned Task.
        /// For example if <see cref="@this"/> is a throwing synchronous delegate,
        /// the exception will not be immediately throwed to the caller, but it will be put inside the returned <see cref="Task.Exception"/>.
        /// It's only when the caller applies the await operator over the returned Task, that the exception will be rethrowed to the caller
        /// </summary>
        /// <exception cref="ArgumentNullException"><param name="this">is null</param></exception>
        public static Task<T> InvokeAsync<T>(Func<T> @this)
        {
            if (@this is null)
                throw new ArgumentNullException(nameof(@this));

            return Core();

            async Task<T> Core()
            {
                var ret = @this();
                return await Task.FromResult(ret).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// This method immediatly invokes <see cref="@this"/> and ensures
        /// that all the exceptions that it throws are wrapped inside the returned Task.
        /// For example if <see cref="@this"/> is a throwing synchronous delegate,
        /// the exception will not be immediately throwed to the caller, but it will be put inside the returned <see cref="Task.Exception"/>.
        /// It's only when the caller applies the await operator over the returned Task, that the exception will be rethrowed to the caller
        /// </summary>
        /// <exception cref="ArgumentNullException"><param name="this">is null</param></exception>
        public static Task InvokeAsync(Action @this)
        {
            if (@this is null)
                throw new ArgumentNullException(nameof(@this));

            return Core();

            async Task Core()
            {
                @this();
                await Task.CompletedTask.ConfigureAwait(false);
            }
        }


        public static async Task WaitUntilCancellationAsync(this CancellationToken @this)
        {
            var tcs = new TaskCompletionSource<object?>(TaskCreationOptions.RunContinuationsAsynchronously);
            await using var _ = @this.Register(() => tcs.TrySetCanceled()).ConfigureAwait(false);
            if (@this.IsCancellationRequested)
                tcs.TrySetCanceled();

            await tcs.Task.ConfigureAwait(false);
        }

        public static async Task Finally(this Task @this, Action<Task> action, bool continueOnCapturedContext = true)
        {
            if (@this is null) 
                throw new ArgumentNullException(nameof(@this));

            if (action is null) 
                throw new ArgumentNullException(nameof(action));

            try
            {
                await @this.ConfigureAwait(continueOnCapturedContext);
            }
            finally
            {
                action(@this);
            }
        }

        public static async Task Finally(this Task @this, Func<Task, Task> action, bool continueOnCapturedContext = true)
        {
            if (@this is null) 
                throw new ArgumentNullException(nameof(@this));

            if (action is null) 
                throw new ArgumentNullException(nameof(action));

            try
            {
                await @this.ConfigureAwait(continueOnCapturedContext);
            }
            finally
            {
                await action(@this).ConfigureAwait(false);
            }
        }
    }
}
