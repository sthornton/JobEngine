using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Common
{
    public interface ICacheProvider
    {
        Type GetCachedItem<Type>(string key) where Type : class;

        void AddCacheItem(string key, int expirationInSeconds, object item);

        void RemoveCacheItem(string key);
    }
}
