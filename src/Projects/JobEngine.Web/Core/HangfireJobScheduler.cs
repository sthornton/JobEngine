using JobEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hangfire;
using JobEngine.Persistence;

namespace JobEngine.Web
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