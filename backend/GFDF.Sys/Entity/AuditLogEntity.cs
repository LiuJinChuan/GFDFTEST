using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("base_auditlog")]
    public class AuditLogEntity : BaseEntity
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string userid { get; set; } = "";


        /// <summary>
        /// 控制器
        /// </summary>
        public string controller { get; set; } = "";


        /// <summary>
        /// 行为
        /// </summary>
        public string action { get; set; } = "";


        /// <summary>
        /// 参数
        /// </summary>
        public string parameters { get; set; } = "";


        /// <summary>
        /// 异常
        /// </summary>
        public string exception { get; set; } = "";


        /// <summary>
        /// useragent
        /// </summary>
        public string useragent { get; set; } = "";


        /// <summary>
        /// 结果
        /// </summary>
        public string result { get; set; } = "";


        /// <summary>
        /// ip
        /// </summary>
        public string ip { get; set; } = "";


        /// <summary>
        /// 主机
        /// </summary>
        public string host { get; set; } = "";


        /// <summary>
        /// 持续时间
        /// </summary>
        public long duration { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        [Write(false)]
        public long createtime => GFContext.idworker.GetTime(id);
    }
}
