using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("sys_dbtablefield")]
    public class DBTableFieldEntity : BaseEntity
    {
        /// <summary>
        /// �ֶ�����Ӣ�ģ�
        /// </summary> 
        public string ename { get; set; }

        /// <summary>
        /// �ֶ��������ģ�
        /// </summary> 
        public string cname { get; set; } = "";

        /// <summary>
        /// �ֶ�����
        /// </summary> 
        public string ctype { get; set; }

        /// <summary>
        /// �ֶγ���
        /// </summary> 
        public int clength { get; set; } = 0;

        /// <summary>
        /// �ֶξ���
        /// </summary> 
        public int accuracy { get; set; } = 0;

        /// <summary>
        /// ����Ϊ��
        /// </summary> 
        public bool allownull { get; set; } = false;

        /// <summary>
        /// �������ݱ�
        /// </summary> 
        public long bttable { get; set; } = 0;

        /// <summary>
        /// �������ݿ�
        /// </summary> 
        public long btdatabase { get; set; } = 0;

        /// <summary>
        /// ����������
        /// </summary> 
        public long btserver { get; set; } = 0;
    }
}
