using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_formvalue")]
    public class FormValueEntity : BaseEntity
    {
        /// <summary>
        /// 表单值名称
        /// </summary>
        public string cname { get; set; } = "";


        /// <summary>
        /// 模块
        /// </summary>
        public string module { get; set; }


        /// <summary>
        /// 表单id
        /// </summary>
        public long formid { get; set; }


        /// <summary>
        /// 调用标识
        /// </summary>
        public string callcode { get; set; } = "";


        /// <summary>
        /// 备注
        /// </summary>
        public string memo { get; set; } = "";
    }
}
