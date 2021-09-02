using Dapper.Extensions;
using GFDF.Infrastruct.Core;
using SysContext = GFDP.Sys.GFContext;

namespace GFDP.Sys.Entity
{
    [Table("sys_message")]
    public class MessageEntity : BaseEntity
    {
        /// <summary>
        /// 消息键
        /// </summary>
        public string code { get; set; } = "";


        /// <summary>
        /// 描述
        /// </summary>
        public string msg { get; set; } = "";


        /// <summary>
        /// 业务数据
        /// </summary>
        public string busdata { get; set; } = "";


        /// <summary>
        /// 创建时间
        /// </summary>
        public long createtime => SysContext.idworker.GetTime(id);
    }
}
