using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFDF.Infrastruct.Extensions
{
    public static class DateTimeExt
    {
        /// <summary> 
        /// 获取指定时间的Unix时间戳 10位
        /// </summary> 
        /// <returns></returns> 
        public static long GetTimeStampTen(this DateTime date)
        {
            return (date.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        /// <summary>  
        /// 获取时间戳  13位
        /// </summary>  
        /// <returns></returns>  
        public static long GetTimeStamp(this DateTime date)
        {
            return (date.ToUniversalTime().Ticks - 621355968000000000) / 10000;
        }


        /// <summary>  
        /// 13位时间戳 转时间  
        /// </summary>  
        /// <returns></returns>  
        public static DateTime GetDateTime(this long stamp)
        {
            DateTime StartDateTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 1, 1, 1), TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
            return StartDateTime.AddMilliseconds(stamp);
        }

        /// <summary>  
        /// 10位时间戳 转时间  
        /// </summary>  
        /// <returns></returns>  
        public static DateTime GetDateTimeTen(this long stamp)
        {
            DateTime StartDateTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 1, 1, 1), TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"));
            return StartDateTime.AddSeconds(stamp);
        }
    }
}
