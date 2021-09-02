using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_dict")]
    public class DictEntity : BaseEntity
    {
        /// <summary>
        /// �ֶ�����
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
        /// ��ע
        /// </summary> 
        public string memo { get; set; } = "";


        /// <summary>
        /// ���ñ�ʶ
        /// </summary> 
        public string callcode { get; set; } = "";


        /// <summary>
        /// ��ʽ
        /// </summary> 
        public string ccolor { get; set; } = "";


        /// <summary>
        /// �ֵ�ֵ
        /// </summary> 
        public string cvalue { get; set; } = "";
    }
}
