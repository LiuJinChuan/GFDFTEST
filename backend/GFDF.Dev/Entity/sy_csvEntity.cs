using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("data_sy_csv")]
    public class sy_csvEntity : BaseEntity
    {
        //csv名称
        public string cname {get;set;} = "";
        
        //csv路径
        public string csvpath { get; set; } = "";

        //内容
        public string ccontent { get; set; } = "";

        //csv操作员
        public long userid { get; set; }
    }
}
