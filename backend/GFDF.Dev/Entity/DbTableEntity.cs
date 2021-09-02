using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("sys_dbtable")]
    public class DBTableEntity : BaseEntity
    {
        /// <summary>
        /// ������Ӣ�ģ�
        /// </summary> 
        public string ename { get; set; }

        /// <summary>
        /// ���������ģ�
        /// </summary> 
        public string cname { get; set; } = "";

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
