using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobEngine.Web.Areas.Clients.Models
{
    public class JobEngineClientViewModel
    {
        [Key]
        public Guid JobEngineClientId { get; set; }     

        public Guid CustomerId { get; set; }

        public string Name { get; set; }

        [Display(Name="Hostname")]
        public string HostName { get; set; }

        [Display(Name="IP")]
        public string IpAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        [Display(Name="Enabled")]
        public bool IsEnabled { get; set; }

        [Display(Name="Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name="Last Connected")]
        public DateTime? LastConnected { get; set; }

        [Display(Name="Modified")]
        public DateTime DateModified { get; set; }

        [Display(Name="Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name="Created")]
        public DateTime DateCreated { get; set; }
    }
}