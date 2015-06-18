using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using JobEngine.Models;
using JobEngine.Persistence;

namespace JobEngine.Core
{
    public class HangfireJobScheduler : IJobScheduler
    {
        public void AddOrUpdate(ScheduledJob scheduledJob)
        {
            RecurringJob.AddOrUpdate<JobQueue>(
                recurringJobId: scheduledJob.Name + "~" + scheduledJob.ScheduledJobId,
                methodCall: x => x.QueueScheduledJob(scheduledJob),
                cronExpression: scheduledJob.CronSchedule);
        }

        public void RemoveIfExists(string jobKey)
        {
            RecurringJob.RemoveIfExists(jobKey);
        }

        public void TriggerNow(string jobKey)
        {
            RecurringJob.Trigger(jobKey);
        }
    }
}
