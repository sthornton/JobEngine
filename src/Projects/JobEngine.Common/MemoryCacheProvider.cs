using System;
using System.Runtime.Caching;

namespace JobEngine.Common
{
    public class MemoryCacheProvider : ICacheProvider
    {
        public Type GetCachedItem<Type>(string key) where Type : class
        {
            ObjectCache cache = MemoryCache.Default;

            Type cachedValue = cache[key] as Type;

            if (cachedValue != null)
            {
                return cachedValue;
            }
            else
            {
                return null;
            }
        }

        public void AddCacheItem(string key, int expirationInSeconds, object item)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now.AddSeconds(expirationInSeconds);
            MemoryCache.Default.Add(key, item, policy);
        }

        public void RemoveCacheItem(string key)
        {
            MemoryCache.Default.Remove(key);
        }
    }
}
