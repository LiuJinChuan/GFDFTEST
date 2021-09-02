using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("adv_advert_release")]
    public class Advert_ReleaseEntity : BaseEntity
    {
        //所属广告位
        public long advertsid { get; set; } = 0;

        
        //所属广告
        public long advertid { get; set; } = 0;


        //发布范围
        public string range { get; set; } = "";

        
        //运行开始时间
        public long run_stime { get; set; } = 1;


        //运行结束时间
        public long run_etime { get; set; } = 1;


        //发布者
        public long userid { get; set; } = 0;


        //创建时间
        public long createtime => GFContext.idworker.GetTime(id);
    }
}
