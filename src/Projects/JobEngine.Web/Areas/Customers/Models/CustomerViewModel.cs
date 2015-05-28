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
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateModified { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
    }
}