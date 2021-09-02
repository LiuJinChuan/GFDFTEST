using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFDF.Infrastruct.Extensions
{
    public static class ObjectExt
    {
        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(this object expression)
        {
            if (expression != null)
                return expression.ToString().IsNumeric();

            return false;
        }

        public static int GetInt(this object obj)
        {
            if (obj == null)
                return 0;
            int.TryParse(obj.ToString(), out int _number);
            return _number;

        }

        /// <summary>
        /// 获取 object 中的枚举值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static long GetLong(this object obj)
        {
            if (obj == null)return 0;
            try
            {
                return Convert.ToInt64(Convert.ToDouble(obj));
            }
            catch
            {
                return 0;
            }
        }

        public static object Add(object obj, string key, object value)
        {
            JObject jObj = JObject.Parse(JsonConvert.SerializeObject(obj));
            jObj.Add(new JProperty(key, value));
            return JsonConvert.DeserializeObject(jObj.ToString());
        }
        public static string ToString2(this object expression)
        {
            return JsonConvert.SerializeObject(expression);
        }

    }
}
