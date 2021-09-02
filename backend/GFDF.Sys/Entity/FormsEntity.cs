using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_forms")]
    public class FormEntity : BaseEntity
    {
        /// <summary>
        /// 表单名称
        /// </summary> 
        public string cname { get; set; } = "";


        /// <summary>
        /// 调用标识
        /// </summary> 
        public string callcode { get; set; } = "";


        /// <summary>
        /// 表单类别
        /// </summary> 
        public string cid { get; set; } = "";


        /// <summary>
        /// 版本
        /// </summary> 
        public int ver { get; set; }


        /// <summary>
        /// 备注
        /// </summary> 
        public string memo { get; set; } = "";
    }
}
