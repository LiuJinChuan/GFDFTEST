using GFDF.Infrastruct.Core;
using SysContext = GFDP.Sys.GFContext;

namespace GFDP.Dev
{
    public class GFContext
    {
        public static void load()
        {
            EventBus.Subscribe(Do);
            
            //JobManager.AddJob(() => SysContext.repository.Execute("update fybx_userext set todaysearch=0"), Schedule => Schedule.WithName("FYBX_clean_todaysearch").ToRunEvery(1).Days().At(0, 1));
        }

        public static void Do(EventBus.EventData eventdata)
        {
            switch (eventdata.EventSource)
            {
                case "common.login":
                    //SysContext.logger.Info("aaaaaaaa");
                    break;
            }
        }
    }
}
