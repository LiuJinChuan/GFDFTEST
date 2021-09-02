using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("qrtz_task")]
    public class QrtzTaskEntity : BaseEntity
    {    
        //任务类型
        public string ctype {get;set;} = "";        

        //名称
        public string cname {get;set;} = "";        

        //所属模块
        public string module {get;set;} = "";        

        //类型
        public string ttype {get;set;} = "";        

        //重复次数
        public decimal  repeatnum {get;set;}         

        //时间间隔
        public long interval {get;set;}         

        //CRON表达式
        public string cron {get;set;} = "";        

        //有效起始时间
        public long cbegin {get;set;}         

        //有效结束时间
        public long cend {get;set;}         

        //认证方式
        public string authway {get;set;} = "";        

        //触发类型
        [Write(false)]
        public int triggertype {
            get
            {
                return (flag & 3);
            }
            set
            {
                flag = (flag & (4095 - 3)) | value;
            }
        }

        //运行状态
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
