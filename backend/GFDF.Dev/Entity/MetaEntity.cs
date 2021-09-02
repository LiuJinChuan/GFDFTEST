using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("sys_meta")]
    public class MetaEntity : BaseEntity
    {
        /// <summary>
        /// 元数据名称
        /// </summary>
        public string cname { get; set; } = "";


        /// <summary>
        /// 元数据名称
        /// </summary>
        public string tname { get; set; } = "";


        /// <summary>
        /// 元数据调用名
        /// </summary>
        public string callcode { get; set; } = "";


        /// <summary>
        /// 备注
        /// </summary>
        public string memo { get; set; } = "";
    }
}
