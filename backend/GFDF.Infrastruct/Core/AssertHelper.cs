using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GFDF.Infrastruct.Core
{
    public class AssertHelper
    {
        public static void EnsureTrue(bool value, string message)
        {
            if (!value) throw new BaseException(message);
        }

        public static void EnsureTrue(bool value, ExEnum exEnum = ExEnum.Other, string message = "", dynamic param = null)
        {
            if (!value) throw new BaseException(exEnum, message, param);
        }

        public static void EnsureFalse(bool value, string message = "")
        {
            EnsureTrue(!value, ExEnum.Other, message);
        }
        public static void EnsureFalse(bool value, ExEnum exEnum = ExEnum.Other, string message = "", dynamic param = null)
        {
            EnsureTrue(!value, exEnum, message);
        }

        public static void EnsureNotNull(object value, string message = "", dynamic param = null)
        {
            EnsureFalse(value == null, ExEnum.ArgNullEx, message);
            Type type = value.GetType();
            if (type.IsValueType && value.Equals(Activator.CreateInstance(type))) throw new BaseException(ExEnum.ArgRangeEx, message, param);
            //    switch (type.Name)
            //{
            //    default:
            //        break;

            //    case "Int32":
            //    case "Int64":
            //        if ((Int64)value == 0) throw new BaseException(ExEnum.ArgRangeEx);
            //        break;
            //}
        }

        public static void EnsureNull(object value, string message = "") => EnsureTrue(value == null, ExEnum.ExistEx, message);

        public static void EnsureNotFound(object value, string message = "") => EnsureTrue(value == null, ExEnum.NotExistEx, message);

        public static void EnsureNotEmpty<T>(IEnumerable<T> collection, string message = "") => EnsureFalse((collection == null || collection.Count() == 0), ExEnum.ArgNullEx, message);

        public static void NotNullOrWhiteSpace(string param, string message = "") => EnsureFalse(string.IsNullOrWhiteSpace(param), ExEnum.ArgNullEx, message);

        public static void EnsureRegMatch(string strRegex, string value, string message = "") => EnsureTrue(Regex.IsMatch(value, strRegex), ExEnum.ArgFormatEx, message);
    }
}
