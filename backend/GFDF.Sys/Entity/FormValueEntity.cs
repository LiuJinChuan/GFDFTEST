using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_formvalue")]
    public class FormValueEntity : BaseEntity
    {
        public string cname { get; set; } = "";

        //模块
        public string module { get; set; }

        public long formid { get; set; }

        public string callcode { get; set; } = "";

        public string memo { get; set; } = "";
    }
}
