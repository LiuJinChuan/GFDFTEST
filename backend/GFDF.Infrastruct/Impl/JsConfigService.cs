using System;
using System.Reactive.Subjects;
using GFDF.Infrastruct.Core;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.Caching;

namespace GFDF.Infrastruct.Impl
{
    public class JsConfigService
    {
        private Dictionary<string, object> jsconfig { get; set; } = new Dictionary<string, object> { { "version", 1.0 } };
        private string path = AppDomain.CurrentDomain.BaseDirectory + "jsconfig.json";

        public JsConfigService()
        {
            if (System.IO.File.Exists(path)) jsconfig = JsonConvert.DeserializeObject<Dictionary<string, object>>(System.IO.File.ReadAllText(path));
            else System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(jsconfig));
        }

        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default(T);
            }
            return (T)jsconfig[key];
        }
    }
}
