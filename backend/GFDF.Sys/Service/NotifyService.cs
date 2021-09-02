using Dapper.Extensions;
using GFDF.Infrastruct.Core;
using GFDP.Sys.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GFDP.Sys.Service
{
    public interface INotifyMsg { }
    //通知发布 
    public interface INotificationfPublisher
    {
         void PublishAsync();
    }
    public class publish { }


}
