using Dapper.Extensions;
using Newtonsoft.Json;

namespace GFDF.Infrastruct.Core
{
    public class BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>  
        [ExplicitKey]
        public long id { get; set; }

        //数据状态  IfModifiedSince   logdel  lock  
        public int cstatus { get; set; }  

        //数据标志
        public int flag { get; set; }

        //扩展
        public string ext { get; set; } = "{}";

        [Write(false)]
        public dynamic extobj {
            get{
                return string.IsNullOrEmpty(ext) ? ext : JsonConvert.DeserializeObject(ext);
            }
            set
            {
                ext = JsonConvert.SerializeObject(value);
            }
        }

        [Write(false)]
        public bool isdel => (cstatus & 2) > 0;
        [Write(false)]
        public bool islock => (cstatus & 4) > 0;
    }
}
