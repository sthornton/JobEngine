using AutoMapper;
using JobEngine.Core.Persistence;
using JobEngine.Models;
using JobEngine.Web.Areas.Clients.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace JobEngine.Web.Areas.Dashboard.Controllers
{
    public class HomeController : BaseController
    {
        private IClientRepository clientRepository;

        public HomeController(IClientRepository clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public ActionResult Index()
        {
            var clients = this.clientRepository.GetAll();
            clients = clients.Where(x => x.IsEnabled && !x.IsDeleted);
            List<JobEngineClientViewModel> viewModel = Mapper.Map<List<JobEngineClient>, List<JobEngineClientViewModel>>(clients.ToList());
            return View(viewModel.OrderBy(x => x.Name));
        }
    }
}