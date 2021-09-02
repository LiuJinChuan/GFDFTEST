using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFDF.Infrastruct.Extensions
{
    public static class EnumExt
    {
        /// <summary>  
        /// 获取枚举的显示内容  
        /// </summary>  
        /// <param name="en">枚举</param>  
        /// <returns>返回枚举的描述</returns>  
        //    public static string GetDisplayContent(this Enum en)
        //    {
        //        var type = en.GetType();   //获取类型  
        //        var memberInfos = type.GetMember(en.ToString());   //获取成员  
        //        if (memberInfos != null && memberInfos.Length > 0)
        //        {
        //            //获取特性  
        //            if (memberInfos[0].GetCustomAttributes(typeof(DisplayAttribute), false) is DisplayAttribute[] attrs && attrs.Length > 0)
        //            {
        //                return attrs[0].Name ?? attrs[0].Description;    //返回当前名称  
        //            }
        //        }
        //        return en.ToString();
        //    }
        //}
    }
}
