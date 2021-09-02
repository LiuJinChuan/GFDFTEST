using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("sys_formins")]
    public class FormInsEntity : BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary> 
        public string cname { get; set; } = "";


        /// <summary>
        /// 业务编码
        /// </summary> 
        public string bno { get; set; } = "";


        /// <summary>
        /// 调用标识
        /// </summary> 
        public string callcode { get; set; } = "";


        /// <summary>
        /// 表单id
        /// </summary>
        public long formid { get; set; }


        /// <summary>
        /// 创建者
        /// </summary>
        public long creator { get; set; }


        /// <summary>
        /// 范围配置
        /// </summary> 
        public string scopecfg { get; set; } = "";


        /// <summary>
        /// 备注
        /// </summary> 
        public string memo { get; set; } = "";
    }
}
