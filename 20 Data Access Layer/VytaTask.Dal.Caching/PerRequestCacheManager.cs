﻿using System.Collections;
using System.Linq;
using System.Web;
using VytaTask.Business.Contracts.Infrastructure;

namespace VytaTask.Dal.Caching
{
    /// <summary>
    /// Represents a manager for caching during an HTTP request (short term caching)
    /// </summary>
    public class PerRequestCacheManager : ICacheManager
    {
        private readonly HttpContextBase _context;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Context</param>
        public PerRequestCacheManager(HttpContextBase context)
        {
            this._context = context;
        }

        /// <summary>
        /// Creates a new instance of the MrRequestCache class
        /// </summary>
        protected virtual IDictionary GetItems()
        {
            return _context?.Items;
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        public virtual T Get<T>(string key)
        {
            var items = GetItems();
            if (items == null)
                return default(T);

            return (T)items[key];
        }

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            var items = GetItems();
            if (items == null || data == null)
                return;

            if (items.Contains(key))
                items[key] = data;
            else
                items.Add(key, data);
        }

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        public virtual bool IsSet(string key)
        {
            var items = GetItems();

            return items?[key] != null;
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">/key</param>
        public virtual void Remove(string key)
        {
            var items = GetItems();

            items?.Remove(key);
        }

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern">pattern</param>
        public virtual void RemoveByPattern(string pattern)
        {
            var items = GetItems();
            if (items == null)
                return;

            this.RemoveByPattern(pattern, items.Keys.Cast<object>().Select(p => p.ToString()));
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public virtual void Clear()
        {
            var items = GetItems();

            items?.Clear();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}