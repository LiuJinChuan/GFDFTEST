using System;
using System.Reactive.Subjects;
using GFDF.Infrastruct.Core;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace GFDF.Infrastruct.Impl
{
    public class CacheService
    {
        private Cache cache { get; set; }

        public CacheService()
        {
            cache = HttpRuntime.Cache;
        }
        
        public bool Contain(string key)
        {
            return cache[key] != null;
        }

        public void Insert(string key, object obj)
        {
            cache.Insert(key, obj);
        }

        public void Insert(string key, object obj, string dependkey)
        {
            //创建缓存依赖项
            CacheDependency dep = new CacheDependency(null, new string[] { dependkey });
            //创建缓存
            cache.Insert(key, obj, dep, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
        }

        /// <summary>
                /// 创建缓存项过期
                /// </summary>
                /// <param name="key">缓存Key</param>
                /// <param name="obj">object对象</param>
                /// <param name="expires">过期时间(分钟)</param>
        public void Insert(string key, object obj, TimeSpan timeout)
        {
            cache.Insert(key, obj, null, Cache.NoAbsoluteExpiration, timeout);
        }
        public void Insert(string key, object obj, DateTime exp)
        {
            cache.Insert(key, obj, null, exp, Cache.NoSlidingExpiration);
        }
        public object Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            return cache.Get(key);
        }

        public Dictionary<string,T> Get<T>(Func<string, bool> expression)
        {
            Dictionary<string, T> list = new Dictionary<string, T>();
            var allKeyList = GetAllKey();
            var getKeyList = allKeyList.Where(expression).ToList();
            foreach (var key in getKeyList)
            {
               list.Add(key,Get<T>(key)); ;
            }
            return list;
        }

        public T Get<T>(string key)
        {
            object obj = Get(key);
            return obj == null ? default(T) : (T)obj;
        }

        public void Remove(string key)
        {
            cache.Remove(key);
        }
        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public void RemoveAll()
        {
            var cacheEnum = GetAllKey();
            foreach(string key in cacheEnum)
            {
                cache.Remove(key);
            }
        }
        public void RemoveAll(Func<string, bool> removeExpression)
        {
            var allKeyList = GetAllKey();
            var delKeyList = allKeyList.Where(removeExpression).ToList();
            foreach (var key in delKeyList)
            {
                cache.Remove(key); ;
            }
        }

        public IEnumerable<string> GetAllKey()
        {
            IDictionaryEnumerator CacheEnum = cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                yield return CacheEnum.Key.ToString();
            }
        }
    }
}
