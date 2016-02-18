using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using JobEngine.Models;
using JobEngine.Core.Persistence;
using JobEngine.Web.Areas.Clients.Models;
using AutoMapper;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using Ionic.Zip;
using System.Configuration;

namespace JobEngine.Web.Areas.Clients.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private IClientRepository clientRepository;
        private ICustomerRepository customerRepository;
        private IClientInstallFilesRepository clientInstallFilesRepository;

        public HomeController(IClientRepository clientRepository, 
                              ICustomerRepository customerRepository,
                              IClientInstallFilesRepository clientInstallRepository)
        {
            this.clientRepository = clientRepository;
            this.customerRepository = customerRepository;
            this.clientInstallFilesRepository = clientInstallRepository;
        }
        public async Task<ActionResult> Index()
        {
            var clients = await this.clientRepository.GetAllAsync();
            var viewModel = Mapper.Map<IEnumerable<JobEngineClient>, IEnumerable<JobEngineClientViewModel>>(clients);
            return View(viewModel.OrderBy(x => x.HostName));
        }

        public async Task<ActionResult> Details(Guid? id)
        {
            if(id.HasValue)
            {
                var client = await this.clientRepository.GetAsync(id.Value);
                var viewModel = Mapper.Map<JobEngineClient, JobEngineClientViewModel>(client);
                return View(viewModel);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);            
        }

        public ActionResult Create()
        {
            CreateJobEngineClientViewModel viewModel = new CreateJobEngineClientViewModel();
            var customers = this.customerRepository.GetAll();
            viewModel.Customers = new SelectList(customers, "CustomerId", "Name");
            viewModel.IsEnabled = true;
            return View(viewModel);
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateJobEngineClientViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Guid clientId = await this.clientRepository.CreateAsync(
                        viewModel.SelectedCustomerId,
                        viewModel.Name,
                        viewModel.IsEnabled,
                        viewModel.Username,
                        viewModel.Password,
                        User.Identity.Name);
                SuccessMessage = "Client created successfully";
                return RedirectToAction("Edit", new { id = clientId });
            }
            var customers = this.customerRepository.GetAll();
            viewModel.Customers = new SelectList(customers, "CustomerId", "Name");
            return View(viewModel);
        }


        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id.HasValue)
            {
                var jobEngineClient = await this.clientRepository.GetAsync(id.Value);
                JobEngineClientViewModel viewModel = Mapper.Map<JobEngineClient, JobEngineClientViewModel>(jobEngineClient);
                return View(viewModel);                
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(JobEngineClientViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var client =  await this.clientRepository.GetAsync(viewModel.JobEngineClientId);
                client.IsEnabled = viewModel.IsEnabled;
                client.Name = viewModel.Name;
                client.Username = viewModel.Username;
                client.Password = viewModel.Password;
                client.ModifiedBy = User.Identity.Name;
                client.DateModified = DateTime.UtcNow;
                await clientRepository.EditAsync(client);
                SuccessMessage = "Client modified successfully";
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }
  
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id.HasValue)
            {
                var client = await this.clientRepository.GetAsync(id.Value);
                JobEngineClientViewModel viewModel = Mapper.Map<JobEngineClient, JobEngineClientViewModel>(client);
                return View(viewModel);
               
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            await this.clientRepository.DeleteAsync(id);
            SuccessMessage = "Deleted successfully";
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Download(Guid? id)
        {
            if(id.HasValue)
            {
                var jobEngineClient = await this.clientRepository.GetAsync(id.Value);
                var installFileDetails = await this.clientInstallFilesRepository.GetActiveInstallerAsync();
                var configFile = System.IO.File.ReadAllText(Server.MapPath("~/Content/Configs/App.config"));
                configFile = configFile.Replace("JOBENGINE_CLIENT_ID", jobEngineClient.JobEngineClientId.ToString());
                configFile = configFile.Replace("REALTIME_URL", ConfigurationManager.AppSettings["RealtimeUrl"]);
                configFile = configFile.Replace("API_URL", ConfigurationManager.AppSettings["ApiUrl"]);
                configFile = configFile.Replace("API_USERNAME", jobEngineClient.Username);
                configFile = configFile.Replace("API_PASSWORD", jobEngineClient.Password);
                configFile = configFile.Replace("TEMP_FILE_DIRECTORY", ConfigurationManager.AppSettings["TempFileDirectory"]);

                Stream outputStream = new MemoryStream();
                Stream stream = new MemoryStream(installFileDetails.File);
                using (var zipFile = ZipFile.Read(stream))
                {
                    zipFile.RemoveEntry("JobEngine.Client.Shell.exe.config");
                    zipFile.AddEntry("JobEngine.Client.Shell.exe.config", configFile);
                    zipFile.Save(outputStream);
                    outputStream.Position = 0;
                    return File(outputStream, "application/zip");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
