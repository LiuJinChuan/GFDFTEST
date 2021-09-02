using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("sys_dbtablefield")]
    public class DBTableFieldEntity : BaseEntity
    {
        /// <summary>
        /// 字段名（英文）
        /// </summary> 
        public string ename { get; set; }


        /// <summary>
        /// 字段名（中文）
        /// </summary> 
        public string cname { get; set; } = "";


        /// <summary>
        /// 字段类型
        /// </summary> 
        public string ctype { get; set; }


        /// <summary>
        /// 字段长度
        /// </summary> 
        public int clength { get; set; } = 0;


        /// <summary>
        /// 字段精度
        /// </summary> 
        public int accuracy { get; set; } = 0;


        /// <summary>
        /// 允许为空
        /// </summary> 
        public bool allownull { get; set; } = false;


        /// <summary>
        /// 所属数据表
        /// </summary> 
        public long bttable { get; set; } = 0;


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
