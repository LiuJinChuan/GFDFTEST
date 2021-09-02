using Dapper.Extensions;
using GFDF.Infrastruct.Core;
using SysContext = GFDP.Sys.GFContext;

namespace GFDP.Sys.Entity
{
    [Table("sys_message")]
    public class MessageEntity : BaseEntity
    {
        //消息键
        public string code { get; set; } = "";


        //描述
        public string msg { get; set; } = "";


        //业务数据
        public string busdata { get; set; } = "";


        //创建时间
        public long createtime => SysContext.idworker.GetTime(id);
    }
}
