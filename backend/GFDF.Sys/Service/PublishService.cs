using GFDF.Infrastruct.Core;
using GFDP.Sys;
using GFDP.Sys.Entity;
using GFDP.Sys.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFDF.Sys.Service
{
    public class PublishService
    {
        private static List<dynamic> msgSettings = JsonConvert.DeserializeObject<List<dynamic>>(JsonConvert.SerializeObject(GFContext.cfg.msg_setting));

        /// <summary>
        /// 发布消息
        /// string entitytype = "", string person = ""
        /// </summary>
        /// <param name="key">消息键</param>
        /// <param name="bus">业务数据</param>
        /// <param name="tousers">收消息人</param>
        /// <param name="data">模板数据</param>
        public static void PublicMsg(string key, string[] tousers, dynamic bus, params object[] data)
        {
            //Akey Bkey 
            var modle = msgSettings.FirstOrDefault(o => (string)o.msg_key == key);
            if (modle == null) return;
            if (tousers == null || tousers.Count() <= 0) return;
            //Akey=> 模板，构建内容    =>msg实体
            string msg = string.Format((string)modle.msg_temp, data);
            long id = GFContext.idworker.nextId();
            GFContext.repository.Insert(new MessageEntity { id = id, code = key, msg = msg, busdata = JsonConvert.SerializeObject(bus) });
            //ToUsers=>     。。。Roles Dept  => msgid-userid关联
            foreach (var touser in tousers)
            {
                GFContext.repository.Insert(new Message_UserEntity { id = GFContext.idworker.nextId(), msgid = id, touser = touser });
                //Bkey=> 触发的EventSource
                string ev = (string)modle.msg_event;
                if (!string.IsNullOrEmpty(ev.Trim())) EventBus.Emit(ev.Trim(), new { msg, touser, msgid = id });
            }
        }
    }
}
