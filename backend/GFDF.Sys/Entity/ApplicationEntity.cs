using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_application")]
    public class ApplicationEntity : BaseEntity
    {
        /// <summary>
        /// 应用名称
        /// </summary> 
        public string cname { get; set; }

        /// <summary>
        /// 应用标识
        /// </summary> 
        public string appsecret { get; set; }

        /// <summary>
        /// 应用描述
        /// </summary> 
        public string description { get; set; } = "";

        /// <summary>
        /// 应用图标
        /// </summary> 
        public string icon { get; set; } = "";
    }
}
