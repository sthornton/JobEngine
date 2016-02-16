using AutoMapper;
using JobEngine.Core.Persistence;
using JobEngine.Models;
using JobEngine.Web.Areas.SiteManagement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JobEngine.Web.Areas.SiteManagement.Controllers
{
    public class ClientInstallFilesController : BaseController
    {
        private IClientInstallFilesRepository clientInstallFileRepository;

        public ClientInstallFilesController(IClientInstallFilesRepository clientInstallFileRepository)
        {
            this.clientInstallFileRepository = clientInstallFileRepository;
        }

        public async Task<ActionResult> Index()
        {
            var files = await clientInstallFileRepository.GetAllAsync();
            var viewModel = Mapper.Map<IEnumerable<ClientInstallFile>, IEnumerable<ClientInstallFileViewModel>>(files);
            return View(viewModel);
        }

        public ActionResult Create()
        {
            return View(new ClientInstallFileViewModel());
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ClientInstallFileViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await this.clientInstallFileRepository.CreateAsync(new ClientInstallFile
                {
                    CreatedBy = User.Identity.Name,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow,
                    File = base.ConvertToBytes(viewModel.File),
                    IsActive = viewModel.IsActive,
                    ModifiedBy = User.Identity.Name,
                    Name = viewModel.Name,
                    Version = viewModel.Version
                });
                SuccessMessage = "Client created successfully";
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                ClientInstallFile clientInstallFile = await this.clientInstallFileRepository.GetDetailsAsync(id.Value);
                ClientInstallFileViewModel viewModel = Mapper.Map<ClientInstallFile, ClientInstallFileViewModel>(clientInstallFile);
                return View("Details", viewModel);
            }
            return RedirectToAction("Index");
        }

        //[HttpGet]
        //public async Task<ActionResult> Edit(int id)
        //{
        //    AssemblyJob assemblyJob = await this.assemblyJobRepository.GetAsync(id);
        //    IEnumerable<AssemblyJobParameter> parameters = await assemblyJobRepository.GetParametersAsync(id);
        //    AssemblyJobViewModel viewModel = Mapper.Map<AssemblyJob, AssemblyJobViewModel>(assemblyJob);
        //    viewModel.AssemblyJobParameters = Mapper.Map<List<AssemblyJobParameter>, List<AssemblyJobParameterViewModel>>(parameters.ToList());
        //    return View("Edit", viewModel);
        //}
    }
}