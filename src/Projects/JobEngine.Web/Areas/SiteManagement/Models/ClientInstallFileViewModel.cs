using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobEngine.Web.Areas.SiteManagement.Models
{
    public class ClientInstallFileViewModel
    {
        [Key]
        public int ClientInstallFileId { get; set; }

        [Required, Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "File")]
        public HttpPostedFileBase File { get; set; }

        [Display(Name = "Version")]
        public string Version { get; set; }

        [Required, Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Date Modified")]
        public DateTime DateModified { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }
    }
}