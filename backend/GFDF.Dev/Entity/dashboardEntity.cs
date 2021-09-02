using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Dev.Entity
{
    [Table("base_dashboard")]
    public class DashboardEntity : BaseEntity
    {
    
        //名称
        public string cname {get;set;} = "";
        

        //调用标识
        public string callcode {get;set;} = "";
        

        //备注
        public string memo {get;set;} 
        

        //所含图表
        public string charts {get;set;} = "[]";
      
    }
}
