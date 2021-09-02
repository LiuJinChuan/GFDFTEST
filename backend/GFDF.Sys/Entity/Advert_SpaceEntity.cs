using Dapper.Extensions;
using GFDF.Infrastruct.Core;

namespace GFDP.Sys.Entity
{
    [Table("adv_advert_space")]
    public class Advert_SpaceEntity : BaseEntity
    {
        //位置名称
        public string cname { get; set; } = "";


        //位置编号
        public string code { get; set; } = "";


        //位置描述
        public string desc { get; set; } = "";


        //所属平台
        public string platform { get; set; } = "";


        //广告类型
        public string ctype { get; set; } = "";


        //展示类型
        public string showtype { get; set; } = "";


        //排序码
        public long sort_id { get; set; } = 1;


        //创建时间
        public long createtime => GFContext.idworker.GetTime(id);
    }
}
