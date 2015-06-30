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

        [Display(Name = "Data Type")]
        public JobEngine.Models.DataType DataType { get; set; }

        [Display(Name = "Required")]
        public bool IsRequired { get; set; }

        [Display(Name = "Encrypted")]
        public bool IsEncrypted { get; set; }

        [Display(Name = "Value")]
        public string Value { get; set; }

        [Display(Name = "Input Validation Expression")]
        public string InputValidationRegExPattern { get; set; }
    }
}