using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("sys_codeseg")]
    public class CodeSegEntity : BaseEntity
    {
        public string cname { get; set; } = "";

        public string callcode { get; set; } = "";

        public string cid { get; set; } = "";

        public string currcode { get; set; } = "";

        public string tags { get; set; } = "";

        public string memo { get; set; } = "";
    }
}
