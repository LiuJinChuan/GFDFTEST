using Dapper.Extensions;
using GFDF.Infrastruct.Core;
using SysContext = GFDP.Sys.GFContext;

namespace GFDP.Sys.Entity
{
    [Table("sys_message_user")]
    public class Message_UserEntity : BaseEntity
    {
        /// <summary>
        /// 消息id
        /// </summary>
        public long msgid { get; set; } = 0;


        /// <summary>
        /// 接受者
        /// </summary>
        public string touser { get; set; } = "";


        /// <summary>
        /// 发布时间
        /// </summary>
        public long releasetime { get; set; } = 0;


        /// <summary>
        /// 阅读时间
        /// </summary>
        public long readtime { get; set; } = 0;


        /// <summary>
        /// 创建时间
        /// </summary>
        public long createtime => SysContext.idworker.GetTime(id);
    }
}
