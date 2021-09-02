using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("sys_dbtable")]
    public class DBTableEntity : BaseEntity
    {
        /// <summary>
        /// 表名（英文）
        /// </summary> 
        public string ename { get; set; }

        /// <summary>
        /// 表名（中文）
        /// </summary> 
        public string cname { get; set; } = "";

        /// <summary>
        /// 所属数据库
        /// </summary> 
        public long btdatabase { get; set; } = 0;

        /// <summary>
        /// 所属服务器
        /// </summary> 
        public long btserver { get; set; } = 0;
    }
}
