using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Core.Persistence
{
    public class RepositoryFactory
    {
        public static IClientRepository GetClientRespository()
        {
            return new ClientRepository(Settings.JobEngineConnectionString);
        }     

        public static IAssemblyJobRepository GetAssemblyJobRepository()
        {
            return new AssemblyJobRepository(Settings.JobEngineConnectionString);
        }

        public static ILoggingRepository GetLoggingRepository()
        {
            return new LoggingRepository(Settings.JobEngineConnectionString);
        }

        public static IJobExecutionQueueRepository GetJobExecutionQueueRepository()
        {
            return new JobExecutionQueueRepository(Settings.JobEngineConnectionString);
        }

        public static IScheduledJobsRepository GetScheduledJobsRepository()
        {
            return new ScheduledJobsRepository(Settings.JobEngineConnectionString);
        }

        public static ICustomerRepository GetCustomerRepository()
        {
            return new CustomerRepository(Settings.JobEngineConnectionString);
        }
    }
}
