using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_dept")]
    public class DeptEntity : BaseEntity
    {
        /// <summary>
        /// 部门名称
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
        /// 业务编码
        /// </summary> 
        public string bno { get; set; } = "";


        /// <summary>
        /// 类型
        /// </summary>
        [Write(false)]
        public string depttype
        {
            get
            {
                return (flag & 15) + "";
            }
            set
            {
                flag = (flag & (4095 - 15)) | int.Parse(value);
            }
        }
    }
}
