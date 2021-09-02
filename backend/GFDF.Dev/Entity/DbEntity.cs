using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("sys_db")]
    public class DBEntity : BaseEntity
    {
        /// <summary>
        /// 库名（英文）
        /// </summary> 
        public string ename { get; set; }


        /// <summary>
        /// 库名（中文）
        /// </summary> 
        public string cname { get; set; } = "";


        /// <summary>
        /// 库类型
        /// </summary> 
        public string ctype { get; set; }


        /// <summary>
        /// 连接字符串
        /// </summary> 
        public string connstr { get; set; }


        /// <summary>
        /// 所属服务器
        /// </summary> 
        public long btserver { get; set; } = 0;
    }
}
