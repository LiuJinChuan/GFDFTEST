using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("adv_advert")]
    public class AdvertEntity : BaseEntity
    {
        //广告名称
        public string cname { get; set; } = "";


        //广告商
        public string advertisers { get; set; } = "";


        //广告商联系电话
        public string telephone { get; set; } = "";


        //链接网址
        public string clink { get; set; } = "";


        //文件地址
        public string cpath { get; set; } = "";


        //广告类型
        public string ctype { get; set; } = "";


        //排序
        public long sort_id { get; set; } = 1;


        //创建时间
        public long createtime => GFContext.idworker.GetTime(id);
    }
}
