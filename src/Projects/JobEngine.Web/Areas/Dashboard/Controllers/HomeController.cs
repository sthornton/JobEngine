﻿using AutoMapper;
using JobEngine.Core.Persistence;
using JobEngine.Models;
using JobEngine.Web.Areas.Clients.Models;
using JobEngine.Web.Areas.Dashboard.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<ActionResult> Index()
        {
            var viewModel = new DashboardViewModel();
            var clients = await this.clientRepository.GetAllAsync();
            clients = clients.Where(x => x.IsEnabled && !x.IsDeleted);
            viewModel.JobEngineClients = Mapper.Map<List<JobEngineClient>, List<JobEngineClientViewModel>>(clients.ToList());
            return View(viewModel);
        }
    }
}