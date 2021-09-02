using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_role")]
    public class RoleEntity : BaseEntity
    {
        /// <summary>
        /// ��ɫ��
        /// </summary> 
        public string cname { get; set; } = "";


        /// <summary>
        /// ��ɫ�ȼ�
        /// </summary> 
        public byte level { get; set; }


        /// <summary>
        /// ���ݷ�Χ
        /// </summary> 
        public byte datascope { get; set; }


        /// <summary>
        /// ����ֵ
        /// </summary> 
        public string datavalue { get; set; } = "";


        /// <summary>
        /// ����
        /// </summary> 
        public int seqno { get; set; }


        /// <summary>
        /// ��ע
        /// </summary> 
        public string memo { get; set; } = "";
    }
}
