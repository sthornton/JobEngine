using AutoMapper;
using JobEngine.Core.Persistence;
using JobEngine.Models;
using JobEngine.Web.Areas.Clients.Models;
using JobEngine.Web.Areas.Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JobEngine.Web.Areas.Dashboard.Controllers
{
    public class HomeController : BaseController
    {
        private IClientRepository clientRepository;
        private IScheduledJobsRepository scheduledJobsRepository;
        private IJobExecutionQueueRepository jobExecutionQueueRepository;

        public HomeController(IClientRepository clientRepository, 
            IScheduledJobsRepository scheduledJobsRepository,
            IJobExecutionQueueRepository jobExecutionQueueRepository)
        {
            this.clientRepository = clientRepository;
            this.scheduledJobsRepository = scheduledJobsRepository;
            this.jobExecutionQueueRepository = jobExecutionQueueRepository;
        }

        public async Task<ActionResult> Index()
        {
            var viewModel = new DashboardViewModel();
            var clients = await this.clientRepository.GetAllAsync();
            clients = clients.Where(x => x.IsEnabled && !x.IsDeleted);
            viewModel.JobEngineClients = Mapper.Map<List<JobEngineClient>, List<JobEngineClientViewModel>>(clients.ToList());
            viewModel.PercSuccessfullScheduleJobs = await this.scheduledJobsRepository.GetPercentageOfSuccessfullJobs(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
            var allClients = await this.clientRepository.GetAllAsync();
            viewModel.JobCountTrend = await this.jobExecutionQueueRepository.GetJobCountTrend(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
            viewModel.JobCountGroupedByClient = await this.jobExecutionQueueRepository.GetJobCountGroupedByClient(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);

            if (allClients != null)
            {
                var online = allClients.Where(x => x.LastConnected > DateTime.UtcNow.AddMinutes(-5));
                if (online != null)
                {
                    var total = allClients.Count();
                    var perc = total > 0 ? online.Count() * 100 / total : 100;
                    viewModel.PercClientsOnline = perc;
                }
            } 
            return View(viewModel);
        }
    }
}