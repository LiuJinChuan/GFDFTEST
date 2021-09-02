using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System;

namespace GFDF.Infrastruct.Extensions
{
    public static class StringExt
    {
        public static string If(this string str, bool condition)
        {
            return condition ? str : string.Empty;
        }
        public static string IfMatch(this string str,string strRegex, bool condition)
        {
            return Regex.IsMatch(str, strRegex) ? str : string.Empty;
        }

        public static string IfNoNull(this string str, string strCheck)
        {
            return !string.IsNullOrWhiteSpace(strCheck) ? str : string.Empty;
        }
        /// <summary>
        /// 截取字符串长度，超出部分使用后缀suffix代替，比如abcdevfddd取前3位，后面使用...代替
        /// </summary>
        /// <param name="orginStr"></param>
        /// <param name="length"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public static string SubStrAddSuffix(this string orginStr, int length, string suffix)
        {
            string ret = orginStr;
            if (orginStr.Length > length)
            {
                ret = orginStr.Substring(0, length) + suffix;
            }
            return ret;
        }


        #region To转换

        public static string ToMd5(this string origin)
        {

            var md5Algorithm = MD5.Create();
            var utf8Bytes = Encoding.UTF8.GetBytes(origin);
            var md5Hash = md5Algorithm.ComputeHash(utf8Bytes);
            var hexString = new StringBuilder();
            foreach (var hexByte in md5Hash)
            {
                hexString.Append(hexByte.ToString("x2"));
            }
            return hexString.ToString();
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool ToBool(this string expression, bool defValue)
        {
            if (expression != null)
            {
                if (string.Compare(expression, "true", true) == 0)
                    return true;
                else if (string.Compare(expression, "false", true) == 0)
                    return false;
            }
            return defValue;
        }


        #endregion


        #region IS判断
        /// 判断对象是否为数字
        public static bool IsNumeric(this string expression) => Regex.IsMatch(expression, @"^[-]?\d+[.]?\d*$");


        /// 检测是否符合email格式
        public static bool IsValidEmail(this string expression) => Regex.IsMatch(expression, @"^[\w\.]+([-]\w+)*@[A-Za-z0-9-_]+[\.][A-Za-z0-9-_]");

        public static bool IsValidDoEmail(this string expression) => Regex.IsMatch(expression, @"^@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

        /// 检测是否是正确的Url
        public static bool IsURL(this string expression) => Regex.IsMatch(expression, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");

        public static bool IsPhone(this string expression) => Regex.IsMatch(expression, @"^[1][3,4,5,7,8][0-9]{9}$");

        #endregion
    }
}
