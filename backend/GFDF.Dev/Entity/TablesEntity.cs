using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("sys_tables")]
    public class TablesEntity : BaseEntity
    {
        /// <summary>
        /// 表名
        /// </summary> 
        public string cname { get; set; } = "";


        /// <summary>
        /// 调用标识
        /// </summary> 
        public string callcode { get; set; } = "";
    }
}
