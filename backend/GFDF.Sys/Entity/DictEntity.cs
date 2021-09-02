using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_dict")]
    public class DictEntity : BaseEntity
    {
        /// <summary>
        /// 字段名称
        /// </summary> 
        public string cname { get; set; } = "";


        /// <summary>
        /// 上级
        /// </summary>
        public long pid { get; set; }


        /// <summary>
        /// 排序
        /// </summary> 
        public int seqno { get; set; }


        /// <summary>
        /// 备注
        /// </summary> 
        public string memo { get; set; } = "";


        /// <summary>
        /// 调用标识
        /// </summary> 
        public string callcode { get; set; } = "";


        /// <summary>
        /// 样式
        /// </summary> 
        public string ccolor { get; set; } = "";


        /// <summary>
        /// 字典值
        /// </summary> 
        public string cvalue { get; set; } = "";
    }
}
