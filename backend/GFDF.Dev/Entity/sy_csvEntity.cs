using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("data_sy_csv")]
    public class Sy_CsvEntity : BaseEntity
    {
        /// <summary>
        /// csv名称
        /// </summary>
        public string cname {get;set;} = "";


        /// <summary>
        /// csv路径
        /// </summary>
        public string csvpath { get; set; } = "";


        /// <summary>
        /// 内容
        /// </summary>
        public string ccontent { get; set; } = "";


        /// <summary>
        /// csv操作员
        /// </summary>
        public long userid { get; set; }
    }
}
