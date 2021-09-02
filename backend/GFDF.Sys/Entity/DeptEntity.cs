using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_dept")]
    public class DeptEntity : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary> 
        public string cname { get; set; } = "";

        /// <summary>
        /// 
        /// </summary> 
        public long pid { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public int seqno { get; set; }

        /// <summary>
        /// 
        /// </summary> 
        public string bno { get; set; } = "";

        //�����쵼
        public long deptleader { get; set; }

        //�ֹ��쵼
        public long incharge { get; set; }

        //����
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
