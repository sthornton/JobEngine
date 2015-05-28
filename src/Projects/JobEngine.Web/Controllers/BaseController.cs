using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobEngine.Web
{
    [Authorize]
    public class BaseController : Controller
    {
        public string SuccessMessage
        {
            set { TempData["SuccessMessage"] = value; }
        }

        public string ErrorMessage
        {
            set { TempData["ErrorMessage"] = value; }
        }
        
    }
}