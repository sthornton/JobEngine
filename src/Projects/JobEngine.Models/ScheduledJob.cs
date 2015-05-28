using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobEngine.Models
{
    public class ScheduledJob
    {
        [Key]
        public int ScheduledJobId { get; set; }

        public Guid JobEngineClientId { get; set; }

        public Guid CustomerId { get; set; }

        public string Name { set; get; }

        public JobType JobType { get; set; }

        public string JobSettings { get; set; }

        public string CronSchedule { get; set; }

        public string LastExecutionResult { get; set; }

        public DateTime? LastExecutionTime { get; set; }

        public long? TotalExecutionTimeInMs { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateCreated { get; set; }
        
        public string CreatedBy { get; set; }

        public DateTime DateModified { get; set; }

        public string ModifiedBy { get; set; }      
    }
}
