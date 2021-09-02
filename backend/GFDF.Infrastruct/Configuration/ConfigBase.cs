
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.IO;
using GFDF.Infrastruct.Extensions;
using System.Collections.Generic;

namespace GFDF.Infrastruct.Configuration
{
    public class CfgManage
    {
        public static CfgManage Instance { get; set; } = new CfgManage();
        private ConcurrentDictionary<string, Func<string, object>> _CfgFuns;
        private ConcurrentDictionary<string, object> ConfigEntries = new ConcurrentDictionary<string, object>();

        private CfgManage()
        {
            _CfgFuns = new ConcurrentDictionary<string, Func<string, object>>();
        }

        public T GetSection<T>(string cfgName)
        {
            var key = GenerateUniqueConfigName(cfgName, typeof(T));
            ConfigEntries.TryGetValue(key, out object entry);
            if (entry == null && _CfgFuns.ContainsKey(key))
            {
                entry = _CfgFuns[key].Invoke(cfgName);
                ConfigEntries.AddOrUpdate(key, entry);
            }
            return (T)entry;
        }

        public T GetSection<T>(string cfgName, Func<string, object> func, bool bf = false)
        {
            var key = GenerateUniqueConfigName(cfgName, typeof(T));
            ConfigEntries.TryGetValue(key, out object entry);
            if (!_CfgFuns.ContainsKey(key)) RegisterGetFunc(cfgName, typeof(T), func);
            if (entry == null)
            {
                entry = func.Invoke(cfgName);
                ConfigEntries.AddOrUpdate(key, entry);
            }
            if (bf)
            {
                entry = func.Invoke(cfgName);
                ConfigEntries.AddOrUpdate(key, entry);
            }
            return (T)entry;
        }

        /// <summary>
        /// 注册获取配置内容的方法
        /// </summary>
        /// <param name="configName"></param>
        /// <param name="type">which use in ConfigBase<'type'></param>
        /// <param name="getRemoteConfigFunction">how to get remote</param>
        public void RegisterGetFunc(string configName, Type type, Func<string, object> getCfgFunc)
        {
            _CfgFuns.AddOrUpdate(GenerateUniqueConfigName(configName, type), getCfgFunc);
        }

        /// <summary>
        /// Generate Unique Config Name (with type) ,No Repeat
        /// </summary>
        /// <param name="configName"></param>
        /// <param name="configActualType">which use in ConfigBase<'type'></param>
        /// <returns></returns>
        protected string GenerateUniqueConfigName(string configName, Type configActualType)
        {
            return string.Concat(configName, "_GFCfgSpliter_", configActualType?.AssemblyQualifiedName?.Replace(" ", "").Replace(",", "").Replace("=", "")?.Trim());
        }
    }

    //public class ConfigBase
    //{
    //    /// <summary>
    //    /// 注册获取配置内容的方法
    //    /// </summary>
    //    /// <param name="configName"></param>
    //    /// <param name="type">which use in ConfigBase<'type'></param>
    //    /// <param name="getRemoteConfigFunction">how to get remote</param>
    //    public static void RegisterGetFunc(string configName, Type type, Func<string,object> getCfgFunc)
    //    {
    //        if (string.IsNullOrEmpty(configName))
    //            throw new ArgumentNullException(nameof(configName), $"{nameof(configName)} must provider!");

    //        CfgManage.Instance.RegisterGetFunc(configName, type, getCfgFunc);
    //    }

    //    public static T GetConfig<T>(string configName) where T : class, new()
    //    {
    //        return CfgManage.Instance.GetSection<T>(configName);
    //    }
    //}
}
