using JobEngine.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using System.Threading.Tasks;
using JobEngine.Models;
using JobEngine.Web.Areas.PowerShellJobs.Models;

namespace JobEngine.Web.Areas.PowerShellJobs.Controllers
{
    public class HomeController : Controller
    {
        private IPowerShellJobsRepository powerShellJobsRepository;
        
        public HomeController(IPowerShellJobsRepository powerShellJobsRepository)
        {
            this.powerShellJobsRepository = powerShellJobsRepository;
        }

        public async Task<ActionResult> Index()
        {
            IEnumerable<PowerShellJob> jobs = await this.powerShellJobsRepository.GetAllAsync();
            var viewModel = Mapper.Map<IEnumerable<PowerShellJob>, IEnumerable<PowerShellJobViewModel>>(jobs);
            return View(viewModel == null ? new List<PowerShellJobViewModel>() : viewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var viewModel = new PowerShellJobViewModel();
            viewModel.Parameters = new List<PowerShellJobParameterViewModel>();
            return View(viewModel);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PowerShellJobViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                var powerShellJob = Mapper.Map<PowerShellJobViewModel, PowerShellJob>(viewModel);
                powerShellJob.DateModified = DateTime.UtcNow;
                powerShellJob.DateCreated = DateTime.UtcNow;
                powerShellJob.ModifiedBy = User.Identity.Name;
                int id = await this.powerShellJobsRepository.CreateAsync(powerShellJob);
                return RedirectToAction("Edit", new { id = id });
            }
            return View(viewModel);
        }

        public async Task<ActionResult> Edit(int id)
        {
            PowerShellJob powerShellJob = await this.powerShellJobsRepository.GetAsync(id);
            PowerShellJobViewModel viewModel = Mapper.Map<PowerShellJob, PowerShellJobViewModel>(powerShellJob);
            return View(viewModel);
        }
    }
}