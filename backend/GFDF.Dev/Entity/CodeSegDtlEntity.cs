using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("sys_codesegdtl")]
    public class CodeSegDtlEntity : BaseEntity
    {
        public string cname { get; set; } = "";

        public string callcode { get; set; } = "";

        public long segid { get; set; }
        public int ver { get; set; }
        public string memo { get; set; } = "";

    }
}
