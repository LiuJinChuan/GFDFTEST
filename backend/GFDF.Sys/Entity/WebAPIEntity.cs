using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_webapi")]
    public class WebApiEntity : BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string cname { get; set; } = "";


        /// <summary>
        /// 模块
        /// </summary>
        public string module { get; set; }


        /// <summary>
        /// 上级类目
        /// </summary>
        public long pid { get; set; }


        /// <summary>
        /// 类别
        /// </summary>
        public string cid { get; set; } = "";


        /// <summary>
        /// 控制器
        /// </summary>
        public string controll { get; set; } = "";


        /// <summary>
        /// 方法
        /// </summary>
        public string action { get; set; } = "";


        /// <summary>
        /// 请求方式
        /// </summary>
        public string method { get; set; } = "";


        /// <summary>
        /// 调用标识
        /// </summary>
        public string callcode { get; set; } = "";


        /// <summary>
        /// 排序
        /// </summary>
        public int seqno { get; set; }
    }
}
