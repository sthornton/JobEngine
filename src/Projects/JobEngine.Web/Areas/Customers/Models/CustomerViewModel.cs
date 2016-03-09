using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobEngine.Web.Areas.Customers.Models
{
    public class CustomerViewModel
    {
        [Key]
        public Guid CustomerId { get; set; }

        [Display(Name="Name")]
        public string Name { get; set; }

        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Modified")]
        public DateTime DateModified { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Created")]
        public DateTime DateCreated { get; set; }
    }
}