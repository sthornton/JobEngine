using JobEngine.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;

namespace JobEngine.Web.Areas.PowerShellJobs.Controllers
{
    public class HomeController : Controller
    {
        private IPowerShellJobsRepository powerShellJobsRepository;
        
        public HomeController(IPowerShellJobsRepository powerShellJobsRepository)
        {
            this.powerShellJobsRepository = powerShellJobsRepository;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}