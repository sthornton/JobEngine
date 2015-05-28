using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using JobEngine.Models;
using JobEngine.Core.Persistence;
using JobEngine.Web.Areas.Clients.Models;
using AutoMapper;

namespace JobEngine.Web.Areas.Clients.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            IClientRepository clientRepository = RepositoryFactory.GetClientRespository();
            var clients = clientRepository.GetAll();
            var viewModel = Mapper.Map<IEnumerable<JobEngineClient>, IEnumerable<JobEngineClientViewModel>>(clients);
            return View(viewModel.OrderBy(x => x.HostName));
        }

        public ActionResult Details(Guid? id)
        {
            if(id.HasValue)
            {
                IClientRepository clientRepository = RepositoryFactory.GetClientRespository();
                var client = clientRepository.Get(id.Value);
                var viewModel = Mapper.Map<JobEngineClient, JobEngineClientViewModel>(client);
                return View(viewModel);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);            
        }

        public ActionResult Create()
        {
            CreateJobEngineClientViewModel viewModel = new CreateJobEngineClientViewModel();
            ICustomerRepository jobEngineRepository = RepositoryFactory.GetCustomerRepository();
            var customers = jobEngineRepository.GetAll();
            viewModel.Customers = new SelectList(customers, "CustomerId", "Name");
            viewModel.IsEnabled = true;
            return View(viewModel);
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateJobEngineClientViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                IClientRepository clientRepository = RepositoryFactory.GetClientRespository();
                Guid clientId = clientRepository.Create(
                        viewModel.SelectedCustomerId,
                        viewModel.Name,
                        viewModel.IsEnabled,
                        viewModel.Username,
                        viewModel.Password,
                        User.Identity.Name);
                SuccessMessage = "Client created successfully";
                return RedirectToAction("Edit", new { id = clientId });
            }
            ICustomerRepository jobEngineRepository = RepositoryFactory.GetCustomerRepository();
            var customers = jobEngineRepository.GetAll();
            viewModel.Customers = new SelectList(customers, "CustomerId", "Name");
            return View(viewModel);
        }


        public ActionResult Edit(Guid? id)
        {
            if (id.HasValue)
            {
                IClientRepository clientRepository = RepositoryFactory.GetClientRespository();
                var jobEngineClient = clientRepository.Get(id.Value);
                JobEngineClientViewModel viewModel = Mapper.Map<JobEngineClient, JobEngineClientViewModel>(jobEngineClient);
                return View(viewModel);                
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JobEngineClientViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                IClientRepository clientRepository = RepositoryFactory.GetClientRespository();
                var client =  clientRepository.Get(viewModel.JobEngineClientId);
                client.IsEnabled = viewModel.IsEnabled;
                client.Name = viewModel.Name;
                client.Username = viewModel.Username;
                client.Password = viewModel.Password;
                client.ModifiedBy = User.Identity.Name;
                client.DateModified = DateTime.UtcNow;
                clientRepository.Edit(client);
                SuccessMessage = "Client modified successfully";
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }
  
        public ActionResult Delete(Guid? id)
        {
            if (id.HasValue)
            {
                IClientRepository clientRepository = RepositoryFactory.GetClientRespository();
                var client = clientRepository.Get(id.Value);
                JobEngineClientViewModel viewModel = Mapper.Map<JobEngineClient, JobEngineClientViewModel>(client);
                return View(viewModel);
               
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            IClientRepository clientRepository = RepositoryFactory.GetClientRespository();
            clientRepository.Delete(id);
            SuccessMessage = "Deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
