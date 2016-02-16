using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobEngine.Web
{
    [Authorize]
    public class BaseController : Controller
    {
        public BaseController() { }

        public string SuccessMessage
        {
            set { TempData["SuccessMessage"] = value; }
        }

        public string ErrorMessage
        {
            set { TempData["ErrorMessage"] = value; }
        }

        public byte[] ConvertToBytes(HttpPostedFileBase file)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(file.InputStream);
            imageBytes = reader.ReadBytes((int)file.ContentLength);
            return imageBytes;
        }
    }
}