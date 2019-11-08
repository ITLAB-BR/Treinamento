using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace ITLab.Treinamento.Api.Core.Cache
{
    public enum CachePriority
    {
        Default = 0,
        NotRemovable = 1
    }

    public class CacheWrapper
    {
        private static ObjectCache cache = MemoryCache.Default;
        private CacheItemPolicy policy = null;
        private CacheEntryRemovedCallback callbackWrapper = null;
        public delegate void callback();
        private callback _callback;

        public void AddToMyCache(String CacheKeyName, Object CacheItem, CachePriority CacheItemPriority, double CacheExpirationSeconds, callback UpdateCache)
        {
            this._callback = UpdateCache;
            AddToMyCache(CacheKeyName, CacheItem, CacheItemPriority, CacheExpirationSeconds);
        }

        public void AddToMyCache(String CacheKeyName, Object CacheItem, CachePriority CacheItemPriority, double CacheExpirationSeconds)
        {
            callbackWrapper = new CacheEntryRemovedCallback(this.CacheWrapperCallback);
            policy = new CacheItemPolicy
            {
                Priority = (System.Runtime.Caching.CacheItemPriority)CacheItemPriority,
                AbsoluteExpiration = DateTime.Now.AddSeconds(CacheExpirationSeconds),
                RemovedCallback = callbackWrapper
            };
            //policy.ChangeMonitors.Add(new HostFileChangeMonitor(FilePath));

            cache.Set(CacheKeyName, CacheItem, policy);
        }

        public static Object GetCachedItem(String CacheKeyName)
        {
            return cache[CacheKeyName] as Object;
        }

        public static void RemoveCachedItem(String CacheKeyName)
        {
            if (cache.Contains(CacheKeyName))
            {
                cache.Remove(CacheKeyName);
            }
        }

        private void CacheWrapperCallback(CacheEntryRemovedArguments arguments)
        {
            //String strLog = String.Concat("Reason: ", arguments.RemovedReason.ToString(), " | Key - Name: ", arguments.CacheItem.Key, " | Value - Object: ", arguments.CacheItem.Value.ToString());
            if (_callback != null)
            {
                _callback();
            }
        }
        public static List<KeyValuePair<string, object>> GetAllItemsCached()
        {
            var cacheItems = new List<KeyValuePair<string, object>>();

            foreach (var item in MemoryCache.Default)
            {
                cacheItems.Add(item);
            }

            return cacheItems;
        }
    }
}