#nullable enable

using System.Diagnostics.CodeAnalysis;

namespace FaceShuffle.Models.Generic
{
    /// <summary>
    /// This struct represent a value that can either be or not be there.
    /// It's similar to Nullable, but Nullable does not work for reference types.
    /// Useful in generic methods for write code once for both value and reference types.
    /// </summary>
    /// <typeparam name="T">The type of the optional object</typeparam>
    public readonly struct Optional<T>
    {
        readonly T? _value;
        public Optional() => (_value, HasValue) = (default, false);
        public Optional(T value) => (_value, HasValue) = (value ?? throw new ArgumentNullException(nameof(value)), true);
        public static Optional<T> Empty => new();

        public static implicit operator Optional<T>(T value) => new(value);

        public bool HasValue { get; }
        public T Value => HasValue ? _value! : throw new InvalidOperationException("Impossible retrieve a value for an empty optional");
    }

    /// <summary>
    /// Provides extension methods for the <see cref="Optional{T}"/> class.
    /// </summary>
    public static class OptionalExtensions
    {
        /// <summary>
        /// Tries to retrieve the value from an Optional.
        /// </summary>
        /// <typeparam name="T">The type of the value in the Optional.</typeparam>
        /// <param name="this">The Optional value.</param>
        /// <param name="value">The retrieved value, or the default value if the Optional is empty.</param>
        /// <returns><see langword="true"/> if the Optional has a value; otherwise, <see langword="false"/>.</returns>
        public static bool TryGetValue<T>(this Optional<T> @this, [NotNullWhen(true)] out T? value)
        {
            value = @this.HasValue ? @this.Value : default;
            return @this.HasValue;
        }

        /// <summary>
        /// Projects the value of an Optional into a new Optional with a different type,
        /// or returns an empty Optional if the source Optional is empty.
        /// </summary>
        /// <typeparam name="TSource">The type of the value in the source Optional.</typeparam>
        /// <typeparam name="TResult">The type of the value in the resulting Optional.</typeparam>
        /// <param name="this">The source Optional value.</param>
        /// <param name="selector">The projection function to apply to the value.</param>
        /// <returns>The projected Optional value, or an empty Optional if the source Optional is empty.</returns>
        public static Optional<TResult> SelectOrEmpty<TSource, TResult>(this Optional<TSource> @this, Func<TSource, TResult> selector)
        {
            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return @this.HasValue ? selector(@this.Value) : default!;
        }

        /// <summary>
        /// Projects the value of an Optional into a new Optional with a different type,
        /// or returns an empty Optional if the source Optional is empty.
        /// </summary>
        /// <typeparam name="TSource">The type of the value in the source Optional.</typeparam>
        /// <typeparam name="TResult">The type of the value in the resulting Optional.</typeparam>
        /// <param name="this">The source Optional value.</param>
        /// <param name="selector">The projection function to apply to the value.</param>
        /// <returns>The projected Optional value, or an empty Optional if the source Optional is empty.</returns>
        public static Optional<TResult> SelectOrEmpty<TSource, TResult>(this Optional<TSource> @this, Func<TSource, Optional<TResult>> selector)
        {
            if (selector is null)
                throw new ArgumentNullException(nameof(selector));

            return @this.SelectOrEmpty<TSource, Optional<TResult>>(selector).Unwrap();
        }

        /// <summary>
        /// Filters an Optional value based on a provided filter function.
        /// </summary>
        /// <typeparam name="T">The type of the value in the Optional.</typeparam>
        /// <param name="this">The Optional value to filter.</param>
        /// <param name="filter">The filter function to apply to the Optional value.</param>
        /// <returns>The filtered Optional value.</returns>
        public static Optional<T> Where<T>(this Optional<T> @this, Func<T, bool> filter)
        {
            if (filter is null)
                throw new ArgumentNullException(nameof(filter));

            return @this.HasValue && filter(@this.Value) ? @this : Optional<T>.Empty;
        }

        /// <summary>
        /// Unwraps a nested Optional value.
        /// </summary>
        /// <typeparam name="T">The type of the value in the nested Optional.</typeparam>
        /// <param name="this">The nested Optional value to unwrap.</param>
        /// <returns>The unwrapped Optional value, or the default value if the nested Optional is empty.</returns>
        public static Optional<T> Unwrap<T>(this Optional<Optional<T>> @this)
        {
            if (@this.TryGetValue(out var nestedOptional) && nestedOptional.TryGetValue(out var value))
                return value;

            return default;
        }

        /// <summary>
        /// Converts an Optional value to an Optional value with non-null values only.
        /// </summary>
        /// <typeparam name="T">The type of the value in the Optional.</typeparam>
        /// <param name="this">The Optional value to convert.</param>
        /// <returns>An Optional value without null values.</returns>
        public static Optional<T> EmptyIfNull<T>(this Optional<T> @this)
        {
            return @this.Where(x => x is not null);
        }

        /// <summary>
        /// Executes an action based on the presence or absence of a value in the Optional.
        /// </summary>
        /// <typeparam name="T">The type of the value in the Optional.</typeparam>
        /// <param name="this">The Optional value.</param>
        /// <param name="onSome">The action to execute if the Optional value is not empty.</param>
        /// <param name="onEmpty">The action to execute if the Optional value is empty.</param>
        public static void Switch<T>(this Optional<T> @this, Action<T> onSome, Action onEmpty)
        {
            if (onSome is null)
                throw new ArgumentNullException(nameof(onSome));

            if (onEmpty is null)
                throw new ArgumentNullException(nameof(onEmpty));

            if (@this.TryGetValue(out var value))
            {
                onSome(value);
                return;
            }

            onEmpty();
        }


        /// <summary>
        /// Matches an Optional value to a result value based on the presence or absence of the value.
        /// </summary>
        /// <typeparam name="TSource">The type of the value in the Optional.</typeparam>
        /// <typeparam name="TDest">The type of the result value.</typeparam>
        /// <param name="this">The Optional value to match.</param>
        /// <param name="onSome">The function to map the value if the Optional is not empty.</param>
        /// <param name="onEmpty">The function to map the value if the Optional is empty.</param>
        /// <returns>The result value based on the matching operation.</returns>
        public static TDest Match<TSource, TDest>(this Optional<TSource> @this, Func<TSource, TDest> onSome, Func<TDest> onEmpty)
        {
            if (onSome is null)
                throw new ArgumentNullException(nameof(onSome));

            if (onEmpty is null)
                throw new ArgumentNullException(nameof(onEmpty));

            return @this.SelectOrEmpty(onSome).GetValueOr(onEmpty);
        }

        /// <summary>
        /// Asynchronously executes an action based on the presence or absence of a value in the Optional.
        /// </summary>
        /// <typeparam name="T">The type of the value in the Optional.</typeparam>
        /// <param name="this">The Optional value.</param>
        /// <param name="onSome">The asynchronous action to execute if the Optional value is not empty.</param>
        /// <param name="onEmpty">The asynchronous action to execute if the Optional value is empty.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task Switch<T>(this Optional<T> @this, Func<T, Task> onSome, Func<Task> onEmpty)
        {
            if (onSome is null)
                throw new ArgumentNullException(nameof(onSome));

            if (onEmpty is null)
                throw new ArgumentNullException(nameof(onEmpty));

            if (@this.TryGetValue(out var value))
            {
                await onSome(value).ConfigureAwait(false);
                return;
            }

            await onEmpty().ConfigureAwait(false);
        }

        /// <summary>
        /// Converts a nullable value to an Optional value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="this">The nullable value to convert.</param>
        /// <returns>An Optional value representing the nullable value.</returns>
        public static Optional<T> ToOptional<T>(this T? @this)
        {
            return @this is null ? Optional<T>.Empty : new(@this);
        }

        /// <summary>
        /// Converts a nullable value to an Optional value based on a condition.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="this">The nullable value to convert.</param>
        /// <param name="notEmptyCondition">The condition to determine if the value is not empty.</param>
        /// <returns>An Optional value representing the nullable value if the condition is met; otherwise, an empty Optional.</returns>
        public static Optional<T> ToOptional<T>(this T @this, Func<T, bool> notEmptyCondition)
        {
            if (notEmptyCondition is null)
                throw new ArgumentNullException(nameof(notEmptyCondition));

            return notEmptyCondition(@this) ? @this : Optional<T>.Empty;
        }

        /// <summary>
        /// Retrieves the value from an Optional, or returns a default value if the Optional is empty.
        /// </summary>
        /// <typeparam name="T">The type of the value in the Optional.</typeparam>
        /// <param name="this">The Optional value.</param>
        /// <param name="defaultValue">The function to provide the default value.</param>
        /// <returns>The value from the Optional if it has a value; otherwise, the default value.</returns>
        public static T GetValueOr<T>(this Optional<T> @this, Func<T> defaultValue)
        {
            if (defaultValue is null)
                throw new ArgumentNullException(nameof(defaultValue));

            return @this.HasValue ? @this.Value : defaultValue();
        }

        /// <summary>
        /// Retrieves the value from an Optional, or returns the default value if the Optional is empty.
        /// </summary>
        /// <typeparam name="T">The type of the value in the Optional.</typeparam>
        /// <param name="this">The Optional value.</param>
        /// <returns>The value from the Optional if it has a value; otherwise, the default value.</returns>
        public static T? GetValueOrDefault<T>(this Optional<T> @this) => @this.GetValueOrDefault(default);

        /// <summary>
        /// Retrieves the value from an Optional, or returns the specified default value if the Optional is empty.
        /// </summary>
        /// <typeparam name="T">The type of the value in the Optional.</typeparam>
        /// <param name="this">The Optional value.</param>
        /// <param name="defaultValue">The default value to return if the Optional is empty.</param>
        /// <returns>The value from the Optional if it has a value; otherwise, the specified default value.</returns>
        public static T? GetValueOrDefault<T>(this Optional<T> @this, T? defaultValue)
        {
            return @this.HasValue ? @this.Value : defaultValue;
        }

        /// <summary>
        /// Retrieves the value from an Optional, or returns an empty list if the Optional is empty.
        /// </summary>
        /// <typeparam name="T">The type of the values in the list.</typeparam>
        /// <param name="this">The Optional list.</param>
        /// <returns>The value from the Optional if it has a value; otherwise, an empty list.</returns>
        public static List<T> GetValueOrEmptyList<T>(this Optional<List<T>> @this) => @this.GetValueOr(static () => new List<T>());

        /// <summary>
        /// Retrieves the value from an Optional, or returns an empty read-only list if the Optional is empty.
        /// </summary>
        /// <typeparam name="T">The type of the values in the list.</typeparam>
        /// <param name="this">The Optional read-only list.</param>
        /// <returns>The value from the Optional if it has a value; otherwise, an empty read-only list.</returns>
        public static IReadOnlyList<T> GetValueOrEmptyList<T>(this Optional<IReadOnlyList<T>> @this) => @this.GetValueOr(static () => Array.Empty<T>());

        /// <summary>
        /// Retrieves the value from an Optional, or returns an empty collection if the Optional is empty.
        /// </summary>
        /// <typeparam name="T">The type of the values in the collection.</typeparam>
        /// <param name="this">The Optional collection.</param>
        /// <returns>The value from the Optional if it has a value; otherwise, an empty collection.</returns>
        public static ICollection<T> GetValueOrEmptyCollection<T>(this Optional<ICollection<T>> @this) => @this.GetValueOr(static () => Array.Empty<T>());

        /// <summary>
        /// Retrieves the value from an Optional, or returns an empty enumerable if the Optional is empty.
        /// </summary>
        /// <typeparam name="T">The type of the values in the enumerable.</typeparam>
        /// <param name="this">The Optional list.</param>
        /// <returns>The value from the Optional if it has a value; otherwise, an empty enumerable.</returns>
        public static IEnumerable<T> GetValueOrEmptyEnumerable<T>(this Optional<List<T>> @this) => @this.GetValueOrDefault() ?? Enumerable.Empty<T>();

        /// <summary>
        /// Retrieves the value from an Optional, or returns an empty enumerable if the Optional is empty.
        /// </summary>
        /// <typeparam name="T">The type of the values in the enumerable.</typeparam>
        /// <param name="this">The Optional read-only list.</param>
        /// <returns>The value from the Optional if it has a value; otherwise, an empty enumerable.</returns>
        public static IEnumerable<T> GetValueOrEmptyEnumerable<T>(this Optional<IReadOnlyList<T>> @this) => @this.GetValueOrDefault() ?? Enumerable.Empty<T>();

        /// <summary>
        /// Retrieves the value from an Optional, or returns an empty enumerable if the Optional is empty.
        /// </summary>
        /// <typeparam name="T">The type of the values in the enumerable.</typeparam>
        /// <param name="this">The Optional enumerable.</param>
        /// <returns>The value from the Optional if it has a value; otherwise, an empty enumerable.</returns>
        public static IEnumerable<T> GetValueOrEmptyEnumerable<T>(this Optional<IEnumerable<T>> @this) => @this.GetValueOrDefault(Enumerable.Empty<T>())!;

        /// <summary>
        /// Returns the first element in the sequence that satisfies the specified condition as an <see cref="Optional{T}"/> object,
        /// or an empty <see cref="Optional{T}"/> object if no such element is found.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="this">The sequence to search.</param>
        /// <param name="filter">A function to test each element for a condition.</param>
        /// <returns>
        /// An <see cref="Optional{T}"/> object containing the first element in the sequence that satisfies the condition,
        /// or an empty <see cref="Optional{T}"/> object if no such element is found.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="this"/> or <paramref name="filter"/> is null.</exception>
        public static Optional<T> FirstOrEmptyOptional<T>(this IEnumerable<T> @this, Func<T, bool> filter)
        {
            if (@this is null)
                throw new ArgumentNullException(nameof(@this));

            if (filter is null)
                throw new ArgumentNullException(nameof(filter));

            return @this.Where(filter).Select(x => new Optional<T>(x)).FirstOrDefault();
        }


        /// <summary>
        /// Retrieves the first element from an enumerable sequence that satisfies the specified condition, or returns an empty Optional if no such element is found.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
        /// <param name="this">The enumerable sequence.</param>
        /// <returns>The first element from the sequence that satisfies the condition, wrapped in an Optional if found; otherwise, an empty Optional.</returns>
        public static Optional<T> FirstOrEmptyOptional<T>(this IEnumerable<T> @this)
        {
            return @this.FirstOrEmptyOptional(static _ => true);
        }

        /// <summary>
        /// Gets the value associated with the specified key from the read-only dictionary as an <see cref="Optional{TValue}"/> object,
        /// or an empty <see cref="Optional{TValue}"/> object if the key is not found.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="this">The read-only dictionary to retrieve the value from.</param>
        /// <param name="key">The key whose value to get.</param>
        /// <returns>
        /// An <see cref="Optional{TValue}"/> object containing the value associated with the specified key,
        /// or an empty <see cref="Optional{TValue}"/> object if the key is not found.
        /// </returns>
        public static Optional<TValue> GetValueOrEmptyOptional<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> @this, TKey key)
        {
            var valueExists = @this.TryGetValue(key, out var value);
            return valueExists ? new Optional<TValue>(value!) : default;
        }

        /// <summary>
        /// Gets the value associated with the specified key from the read-only dictionary as an <see cref="Optional{TValue}"/> object,
        /// or an empty <see cref="Optional{TValue}"/> object if the key is not found.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="this">The read-only dictionary to retrieve the value from.</param>
        /// <param name="key">The key whose value to get.</param>
        /// <returns>
        /// An <see cref="Optional{TValue}"/> object containing the value associated with the specified key,
        /// or an empty <see cref="Optional{TValue}"/> object if the key is not found.
        /// </returns>
        public static Optional<TValue> GetValueOrEmptyOptional<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key)
        {
            var valueExists = @this.TryGetValue(key, out var value);
            return valueExists ? new Optional<TValue>(value!) : default;
        }


        public static async Task SwitchAsync<T>(this Task<Optional<T>> optionalTask, Func<T, Task> onSomeAsync, Func<Task> onEmptyAsync, bool continueOnCalledContext = true)
        {
            if (onSomeAsync is null)
                throw new ArgumentNullException(nameof(onSomeAsync));

            if (onEmptyAsync is null)
                throw new ArgumentNullException(nameof(onEmptyAsync));

            var optional = await optionalTask.ConfigureAwait(continueOnCalledContext);

            if (optional.TryGetValue(out var value))
            {
                await onSomeAsync(value).ConfigureAwait(continueOnCalledContext);
            }
            else
            {
                await onEmptyAsync().ConfigureAwait(continueOnCalledContext);
            }
        }

        public static async Task OnSome<T>(this Task<Optional<T>> optionalTask, Func<T, Task> onSomeAsync, bool continueOnCalledContext = true)
        {
            if (onSomeAsync is null)
                throw new ArgumentNullException(nameof(onSomeAsync));

            var optional = await optionalTask.ConfigureAwait(continueOnCalledContext);

            if (optional.TryGetValue(out var value))
            {
                await onSomeAsync(value).ConfigureAwait(continueOnCalledContext);
            }
        }

        public static async Task OnSome<T>(this Task<Optional<T>> optionalTask, Action<T> onSomeAsync, bool continueOnCalledContext = true)
        {
            if (onSomeAsync is null)
                throw new ArgumentNullException(nameof(onSomeAsync));

            var optional = await optionalTask.ConfigureAwait(continueOnCalledContext);

            if (optional.TryGetValue(out var value))
            {
                onSomeAsync(value);
            }
        }
    }
}

