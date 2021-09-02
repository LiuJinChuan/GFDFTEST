using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("sys_webapi")]
    public class WebapiEntity : BaseEntity
    {
    
        //名称
        public string cname {get;set;} = "";

        //模块
        public string module { get; set; }

        //上级类目
        public long pid {get;set;} 
        

        //类别
        public string cid {get;set;} = "";
        

        //控制器
        public string controll {get;set;} = "";
        

        //方法
        public string action {get;set;} = "";
        

        //请求方式
        public string method {get;set;} = "";
        

        //调用标识
        public string callcode {get;set;} = "";
        

        //排序
        public int seqno {get;set;} 
        
    }
}
