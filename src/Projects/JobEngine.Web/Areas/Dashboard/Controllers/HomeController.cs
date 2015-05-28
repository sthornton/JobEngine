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
        public ActionResult Index()
        {
            IClientRepository clientRepository = RepositoryFactory.GetClientRespository();
            var clients = clientRepository.GetAll();
            clients = clients.Where(x => x.IsEnabled && !x.IsDeleted);
            List<JobEngineClientViewModel> viewModel = Mapper.Map<List<JobEngineClient>, List<JobEngineClientViewModel>>(clients.ToList());
            return View(viewModel.OrderBy(x => x.Name));
        }
    }
}