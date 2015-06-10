using AutoMapper;
using JobEngine.Core.Persistence;
using JobEngine.Models;
using JobEngine.Web.Areas.AssemblyJobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JobEngine.Web.Areas.AssemblyJobs.Controllers
{
    public class HomeController : BaseController
    {
        private IAssemblyJobRepository assemblyJobRepository;

        public HomeController(IAssemblyJobRepository assemblyJobRepository)
        {
            this.assemblyJobRepository = assemblyJobRepository;
        }

        public async Task<ActionResult> Index()
        {
            var assemblyJobs = await this.assemblyJobRepository.GetAllAsync();
            var viewModel = Mapper.Map<List<AssemblyJob>, List<AssemblyJobViewModel>>(assemblyJobs.ToList());
            return View(viewModel);
        }

        public ActionResult Create()       
        {
            return View(new AssemblyJobViewModel());    
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AssemblyJobViewModel viewModel)
        {
            if(viewModel.File.ContentLength > 0)
            {
                viewModel.PluginFileName = Path.GetFileName(viewModel.File.FileName);
                if (ModelState.ContainsKey("PluginFileName"))
                    ModelState["PluginFileName"].Errors.Clear();
            }

            if (ModelState.IsValid)
            {
                if(viewModel.File.ContentLength == 0)
                {
                    ModelState.AddModelError("File", "Please select a File");
                    return View(viewModel);
                }

                if (viewModel.File.ContentLength > 0)
                {
                    AssemblyJob assemblyJob = new AssemblyJob();
                    assemblyJob.Name = viewModel.Name;
                    assemblyJob.PluginName = viewModel.PluginName;
                    assemblyJob.PluginFile = ConvertToBytes(viewModel.File);
                    assemblyJob.PluginFileName = Path.GetFileName(viewModel.File.FileName);
                    assemblyJob.DateCreated = DateTime.UtcNow;
                    assemblyJob.DateModified = DateTime.UtcNow;
                    assemblyJob.ModifiedBy = HttpContext.User.Identity.Name;
                    int assemblyJobId = await this.assemblyJobRepository.CreateAsync(assemblyJob);
                    SuccessMessage = "Assembly Job created successfully";
                    return RedirectToAction("Edit", new { id = assemblyJobId });
                }
            }
            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            AssemblyJob assemblyJob = await this.assemblyJobRepository.GetAsync(id);
            AssemblyJobViewModel viewModel = Mapper.Map<AssemblyJob, AssemblyJobViewModel>(assemblyJob);
            viewModel.AssemblyJobParameters = Mapper.Map<List<AssemblyJobParameter>, List<AssemblyJobParameterViewModel>>(assemblyJob.Parameters);
            return View("Details", viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            AssemblyJob assemblyJob = await this.assemblyJobRepository.GetAsync(id);
            IEnumerable<AssemblyJobParameter> parameters = await assemblyJobRepository.GetParametersAsync(id);
            AssemblyJobViewModel viewModel = Mapper.Map<AssemblyJob, AssemblyJobViewModel>(assemblyJob);
            viewModel.AssemblyJobParameters = Mapper.Map<List<AssemblyJobParameter>, List<AssemblyJobParameterViewModel>>(parameters.ToList());
            return View("Edit", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AssemblyJobViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                AssemblyJob assemblyJob = await this.assemblyJobRepository.GetAsync(viewModel.AssemblyJobId);

                if (viewModel.File != null)
                {
                    assemblyJob.Name = viewModel.Name;
                    assemblyJob.PluginName = viewModel.PluginName;
                    assemblyJob.PluginFile = ConvertToBytes(viewModel.File);
                    assemblyJob.PluginFileName = Path.GetFileName(viewModel.File.FileName);
                }
                if (viewModel.PluginName != assemblyJob.PluginName)
                    assemblyJob.PluginName = viewModel.PluginName;

                assemblyJob.Name = viewModel.Name;
                assemblyJob.DateModified = DateTime.UtcNow;
                assemblyJob.ModifiedBy = User.Identity.Name;
                await this.assemblyJobRepository.EditAsync(assemblyJob);
                return RedirectToAction("Index");

            }
            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> AddParameter(int id)
        {
            AssemblyJob assemblyJob = await this.assemblyJobRepository.GetAsync(id);
            AssemblyJobParameterViewModel viewModel = new AssemblyJobParameterViewModel();
            viewModel.AssemblyJobId = assemblyJob.AssemblyJobId;
            return View("AddParameter", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddParameter(AssemblyJobParameterViewModel viewModel)
        {
            AssemblyJobParameter parameter = new AssemblyJobParameter
            {
                AssemblyJobId = viewModel.AssemblyJobId,
                DataType = DataType.String,
                InputValidationRegExPattern = viewModel.InputValidationRegExPattern,
                IsEncrypted = viewModel.IsEncrypted,
                IsRequired = viewModel.IsRequired,
                Name = viewModel.Name
            };
            this.assemblyJobRepository.CreateParameterAsync(parameter);
            return RedirectToAction("Edit", new { id = viewModel.AssemblyJobId });
        }

        public ActionResult Delete(int id)
        {
            this.assemblyJobRepository.DeleteAsync(id);
            SuccessMessage = "Assembly Job deleted successfully";
            return RedirectToAction("Index");
        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
    }
}