using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("sys_tables")]
    public class TablesEntity : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary> 
        public string cname { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public string callcode { get; set; } = "";
    }
}
