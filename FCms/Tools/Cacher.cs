using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace FCms.Tools
{
    static public class Cacher
    {
        private static MemoryCache cache = null;
        
        static MemoryCache Current {
            get {
                if (cache == null)
                    cache = new MemoryCache("fcms");
                return cache;
            }
        }

        public static void Clear()
        {
            cache = null;
        }

        public static object Get(string cachekey)
        {
            if (Cacher.Current.Contains(cachekey))
                return Cacher.Current.Get(cachekey);
            return null;
        }

        public static long GetCount()
        {
            return Cacher.Current.GetCount();
        }

        public static void Set(string cachekey, object value)
        {
            Set(cachekey, value, int.MaxValue, null, false);
        }

        public static void Set(string cachekey, object value, int seconds, bool sliding, CacheItemPriority priority = CacheItemPriority.Default)
        {
            Set(cachekey, value, seconds, null, sliding, priority);
        }

        public static void Set(string cachekey, object value, string filename)
        {
            Set(cachekey, value, int.MaxValue, new string[] { filename });
        }

        static void Set(string cachekey, object value, int seconds, string[] filedependencies = null, bool sliding = false, CacheItemPriority priority = CacheItemPriority.Default)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            if (filedependencies != null)
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string>(filedependencies)));

            if (sliding)
                policy.SlidingExpiration = new TimeSpan(0, seconds, 0);
            else
                policy.AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(seconds);

            policy.Priority = priority;

            Cacher.Current.Remove(cachekey);
            Cacher.Current.Add(new CacheItem(cachekey, value), policy);
        }

        public static bool Contains(string key)
        {
            return Cacher.Current.Contains(key);
        }

        public static void Remove(string key)
        {
            Cacher.Current.Remove(key);
        }
    }
}
