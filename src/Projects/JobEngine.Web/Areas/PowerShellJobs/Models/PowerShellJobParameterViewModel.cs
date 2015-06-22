using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobEngine.Web.Areas.PowerShellJobs.Models
{
    public class PowerShellJobParameterViewModel
    {
        [Key]
        public int PowerShellJobParameterId { get; set; }

        public int PowerShellJobId { get; set; }

        public string Name { get; set; }

        public DataType DataType { get; set; }

        public bool IsRequired { get; set; }

        public bool IsEncrypted { get; set; }

        public string InputValidationRegExPattern { get; set; }
    }
}