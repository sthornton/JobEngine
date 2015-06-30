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
    public class HomeController : BaseController
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
                SuccessMessage = "Job created successfully";
                return RedirectToAction("Edit", new { id = id });
            }
            return View(viewModel);
        }

        public async Task<ActionResult> Delete(int id)
        {
            await this.powerShellJobsRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DeleteParameter(int powerShellJobId, int powerShellJobParameterId)
        {
            await this.powerShellJobsRepository.DeleteParameterAsync(powerShellJobParameterId);
            return RedirectToAction("Edit", new { id = powerShellJobId });
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            PowerShellJob powerShellJob = await this.powerShellJobsRepository.GetAsync(id);
            PowerShellJobViewModel viewModel = Mapper.Map<PowerShellJob, PowerShellJobViewModel>(powerShellJob);
           viewModel.Parameters = Mapper.Map<List<PowerShellJobParameter>, List<PowerShellJobParameterViewModel>>(powerShellJob.Parameters);
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PowerShellJobViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                var powerShellJob = Mapper.Map<PowerShellJobViewModel, PowerShellJob>(viewModel);
                powerShellJob.DateModified = DateTime.UtcNow;
                powerShellJob.DateCreated = DateTime.UtcNow;
                powerShellJob.ModifiedBy = User.Identity.Name;
                await this.powerShellJobsRepository.EditAsync(powerShellJob);
                SuccessMessage = "Job modified successfully";
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult CreateParameter(int id)
        {
            var viewModel = new PowerShellJobParameterViewModel();
            viewModel.PowerShellJobId = id;
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateParameter(PowerShellJobParameterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var powerShellParameter = Mapper.Map<PowerShellJobParameterViewModel, PowerShellJobParameter>(viewModel);
                await this.powerShellJobsRepository.CreateParameterAsync(powerShellParameter);
                return RedirectToAction("Edit", new { id = viewModel.PowerShellJobId });
            }
            return View(viewModel);
        }
    }
}