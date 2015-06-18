using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;
using JobEngine.Core.Persistence;

namespace JobEngine.Core
{
    public class HangFireSchedulerService : ISchedulerService
    {
        private BackgroundJobServer backgroundServer;

        public void Start(string connString)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage(connString);
            backgroundServer = new BackgroundJobServer();
            InitializeJobs();
        }

        public void Stop()
        {
            backgroundServer.Dispose();
        }

        private void InitializeJobs()
        {
            try
            {
                IScheduledJobsRepository repository = RepositoryFactory.GetScheduledJobsRepository();
                var jobs = repository.GetAll().Result.Where(x => x.IsActive);
                var jobScheduler = new HangfireJobScheduler();
                foreach (var job in jobs)
                {
                    jobScheduler.RemoveIfExists(job.Name + "~" + job.ScheduledJobId);
                    jobScheduler.AddOrUpdate(job);
                }
            }
            catch (System.Exception e)
            {
                throw;
            }
        }
    }
}
