using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_dept")]
    public class DeptEntity : BaseEntity
    {
        /// <summary>
        /// ��������
        /// </summary> 
        public string cname { get; set; } = "";


        /// <summary>
        /// �ϼ�
        /// </summary> 
        public long pid { get; set; }


        /// <summary>
        /// ����
        /// </summary> 
        public int seqno { get; set; }


        /// <summary>
        /// ҵ�����
        /// </summary> 
        public string bno { get; set; } = "";


        /// <summary>
        /// ����
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
