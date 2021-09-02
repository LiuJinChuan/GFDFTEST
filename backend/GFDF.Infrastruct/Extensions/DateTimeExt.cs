using System;
using System.Collections.Generic;
using System.Globalization;
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

        /// <summary>
        /// 阳历转阴历
        /// </summary>
        /// <param name="time">2021-09-01</param>
        /// <returns></returns>
        public static string SolarToLunar(this string time)
        {
            ChineseLunisolarCalendar cncld = new ChineseLunisolarCalendar();
            DateTime dt = DateTime.Parse(time);
            int year = cncld.GetYear(dt);
            int flag = cncld.GetLeapMonth(year);
            int month = flag > 0 ? cncld.GetMonth(dt) - 1 : cncld.GetMonth(dt);
            int day = cncld.GetDayOfMonth(dt);
            return $"{year}-{(month.ToString().Length == 1 ? "0" + month : month + "")}-{(day.ToString().Length == 1 ? "0" + day : day + "")}";
        }

        /// <summary>
        /// 阴历转阳历
        /// </summary>
        /// <param name="time">2021-09-01</param>
        /// <returns></returns>
        public static string LunarToSolar(this string time)
        {
            ChineseLunisolarCalendar cncld = new ChineseLunisolarCalendar();
            int year = Convert.ToInt32(time.Split('-')[0]);
            int month = Convert.ToInt32(time.Split('-')[1]);
            int day = Convert.ToInt32(time.Split('-')[2]);
            int flag = cncld.GetLeapMonth(year);
            DateTime dtnl = cncld.ToDateTime(year, month, day, 0, 0, 0, 0);
            dtnl = flag > 0 ? dtnl.AddMonths(1) : dtnl;
            return dtnl.ToString("yyyy-MM-dd");
        }
    }
}
