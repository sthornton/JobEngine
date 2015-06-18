using JobEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Core
{
    public interface IJobScheduler
    {
        void AddOrUpdate(ScheduledJob scheduledJob);

        void RemoveIfExists(string jobKey);

        void TriggerNow(string jobKey);
    }
}
