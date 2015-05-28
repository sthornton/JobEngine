using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobEngine.Web.Areas.AssemblyJobs.Models
{
    public class AssemblyJobParameterViewModel
    {
        [Key]
        public int AssemblyJobParameterId { get; set; }

        public int AssemblyJobId { get; set; }

        public string Name { get; set; }

        [Display(Name = "Data Type")]
        public JobEngine.Models.DataType  DataType{ get; set; }

        [Display(Name = "Is Required")]
        public bool IsRequired { get; set; }

        [Display(Name = "Is Encrypted")]
        public bool IsEncrypted { get; set; }

        [Display(Name = "Value")]
        public string Value { get; set; }

        [Display(Name = "Rexeg Input Validation Expression")]
        public string InputValidationRegExPattern { get; set; }
    }
}