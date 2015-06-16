using JobEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Core.Persistence
{
    public interface IScheduledJobsRepository
    {
        Task<IEnumerable<ScheduledJob>> GetAll();
        Task<ScheduledJob> Get(int scheduledJobId);
        Task<int> CreateScheduledJob(ScheduledJob scheduledJob);
        Task UpdateScheduledJob(ScheduledJob scheduledJob);
        Task DeleteScheduledJob(int scheduledJobId);
    }
}
