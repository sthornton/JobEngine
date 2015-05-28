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
        IEnumerable<ScheduledJob> GetAll();
        ScheduledJob Get(int scheduledJobId);
        int CreateScheduledJob(ScheduledJob scheduledJob);
        void UpdateScheduledJob(ScheduledJob scheduledJob);
        void DeleteScheduledJob(int scheduledJobId);
    }
}
