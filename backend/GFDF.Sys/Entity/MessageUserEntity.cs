using Dapper.Extensions;
using GFDF.Infrastruct.Core;
using SysContext = GFDP.Sys.GFContext;

namespace GFDP.Sys.Entity
{
    [Table("sys_message_user")]
    public class MessageUserEntity : BaseEntity
    {
        //消息id
        public long msgid { get; set; } = 0;


        //用户
        public string touser { get; set; } = "";


        //发布时间
        public long releasetime { get; set; } = 0;


        //阅读时间
        public long readtime { get; set; } = 0;


        //创建时间
        public long createtime => SysContext.idworker.GetTime(id);
    }
}
