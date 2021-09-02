using FluentScheduler;

namespace GFDP.Sys.Job
{
    internal class ApiJobs :Registry
    {
        public ApiJobs()
        {
            //Schedule(()=>GFContext.logger.Info("job1")).WithName("JobLog").ToRunNow().AndEvery(20).Seconds();
        }
    }
}
