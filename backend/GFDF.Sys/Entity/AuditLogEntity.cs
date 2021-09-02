using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("base_auditlog")]
    public class AuditLogEntity : BaseEntity
    {
        public string userid { get; set; } = "";
        public string controller { get; set; } = "";
        public string action { get; set; } = "";
        public string parameters { get; set; } = "";
        public string exception { get; set; } = "";
        public string useragent { get; set; } = "";
        public string result { get; set; } = "";
        public string ip { get; set; } = "";
        public string host { get; set; } = "";
        public long duration { get; set; }
        [Write(false)]
        public long createtime => GFContext.idworker.GetTime(id);
    }
}
