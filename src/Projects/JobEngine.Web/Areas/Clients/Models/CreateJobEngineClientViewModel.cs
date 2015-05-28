using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobEngine.Web.Areas.Clients.Models
{
    public class CreateJobEngineClientViewModel
    {
        [Display(Name = "Customer")]
        public SelectList Customers { get; set; }

        [Required]
        public Guid SelectedCustomerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Display(Name="Is Enabled")]
        public bool IsEnabled { get; set; }
    }
}