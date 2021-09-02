using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("data_region")]
    public class Data_RegionEntity : BaseEntity
    {
        /// <summary>
        /// ����
        /// </summary> 
        public string cname { get; set; } = "";


        /// <summary>
        /// ��������
        /// </summary> 
        public string code { get; set; }


        /// <summary>
        /// ����
        /// </summary> 
        public string parentcode { get; set; }


        /// <summary>
        /// ���
        /// </summary> 
        public string classcode { get; set; } = "";


        /// <summary>
        /// λ��
        /// </summary> 
        public string localtion { get; set; } = "";


        /// <summary>
        /// λ��
        /// </summary> 
        [Write(false)]
        public string level { get; set; } = "0";


        /// <summary>
        /// ����ʱ��
        /// </summary>
        public long ctime { get; set; } = 0;


        [Write(false)]
        public string jcode { get { return string.IsNullOrEmpty(code) || code == "0" ? "0" : code.TrimEnd('0'); } }


        [Write(false)]
        public string jparentcode { get { return string.IsNullOrEmpty(parentcode) || parentcode == "0" ? "0" : parentcode.TrimEnd('0'); } }
    }
}
