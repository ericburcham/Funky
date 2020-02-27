using System;
using System.Collections.Generic;
using System.Threading;

namespace Funky
{
    /// <summary>
    /// Represents a thread-safe memoizer for a given <see cref="Func{TKey, TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the memoizer's keys.</typeparam>
    /// <typeparam name="TValue">The type of the memoizer's return values.</typeparam>
    /// <typeparam name="TCachedValue">The type of the memoizer's cached values.</typeparam>
    public abstract class MemoizerBase<TKey, TValue, TCachedValue> : IMemoizeThings<TKey, TValue>
    {
        protected MemoizerBase(Func<TKey, TValue> func)
            : this()
        {
            Func = func;
        }

        private MemoizerBase()
        {
            Cache = new Dictionary<TKey, TCachedValue>();
            CacheLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        }

        protected Dictionary<TKey, TCachedValue> Cache { get; }

        protected Func<TKey, TValue> Func { get; }

        private ReaderWriterLockSlim CacheLock { get; }

        /// <summary>
        /// Gets or adds a key/value pair from the memoizer if it already exists.  Adds a key/value pair to the memoizer if it does not already exist.
        /// </summary>
        /// <param name="key">The key of the element to get or add.</param>
        /// <returns>The value for the key.  This will be either the existing value for the key if the key is already in the memoizer, or the new value if the key was not in the memoizer.</returns>
        public TValue GetOrAdd(TKey key)
        {
            TValue result;

            CacheLock.EnterUpgradeableReadLock();
            try
            {
                if (ContainsKey(key))
                {
                    return GetValue(key);
                }
                else
                {
                    Func<TValue> func = () => GetOrSetValue(key);
                    result = func.InvokeWithWriteLock(CacheLock);
                }
            }
            finally
            {
                CacheLock.ExitUpgradeableReadLock();
            }

            return result;
        }

        protected abstract bool ContainsKey(TKey key);

        protected abstract TValue SetValue(TKey key);

        protected abstract TValue GetValue(TKey key);

        private TValue GetOrSetValue(TKey key)
        {
            return ContainsKey(key)
                ? GetValue(key)
                : SetValue(key);
        }
    }
}
