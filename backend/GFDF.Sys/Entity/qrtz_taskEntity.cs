using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("qrtz_task")]
    public class Qrtz_TaskEntity : BaseEntity
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        public string ctype { get; set; } = "";


        /// <summary>
        /// 名称
        /// </summary>
        public string cname { get; set; } = "";


        /// <summary>
        /// 所属模块
        /// </summary>
        public string module { get; set; } = "";


        /// <summary>
        /// 类型
        /// </summary>
        public string ttype { get; set; } = "";


        /// <summary>
        /// 重复次数
        /// </summary>
        public decimal repeatnum { get; set; }


        /// <summary>
        /// 时间间隔
        /// </summary>
        public long interval { get; set; }


        /// <summary>
        /// CRON表达式
        /// </summary>
        public string cron { get; set; } = "";


        /// <summary>
        /// 有效起始时间
        /// </summary>
        public long cbegin { get; set; }


        /// <summary>
        /// 有效结束时间
        /// </summary>
        public long cend { get; set; }


        /// <summary>
        /// 认证方式
        /// </summary>
        public string authway { get; set; } = "";


        /// <summary>
        /// 触发类型
        /// </summary>
        [Write(false)]
        public int triggertype
        {
            get
            {
                return (flag & 3);
            }
            set
            {
                flag = (flag & (4095 - 3)) | value;
            }
        }


        /// <summary>
        /// 运行状态
        /// </summary>
        [Write(false)]
        public int runstate
        {
            get
            {
                return (flag & 12) >> 2;
            }
            set
            {
                flag = (flag & (4095 - 12)) | value;
            }
        }
    }
}
