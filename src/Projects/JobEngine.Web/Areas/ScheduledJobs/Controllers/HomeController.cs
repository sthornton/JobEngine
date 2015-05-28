using AutoMapper;
using JobEngine.Core.Persistence;
using JobEngine.Models;
using JobEngine.Web.Areas.AssemblyJobs.Models;
using JobEngine.Web.Areas.ScheduledJobs.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace JobEngine.Web.Areas.ScheduledJobs.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ICustomerRepository repository = RepositoryFactory.GetCustomerRepository();
            var customers = repository.GetAll();

            IScheduledJobsRepository scheduledJobsRepository = RepositoryFactory.GetScheduledJobsRepository();
            var scheduledJobs = scheduledJobsRepository.GetAll();

            IClientRepository clientRepository = RepositoryFactory.GetClientRespository();
            var clients = clientRepository.GetAll();

            List<ScheduledJobViewModel> viewModel = new List<ScheduledJobViewModel>();
            foreach (var scheduledJob in scheduledJobs)
            {
                var customer = customers.Where(x => x.CustomerId == scheduledJob.CustomerId).FirstOrDefault();
                var jobEngineClient = clients.Where(x => x.JobEngineClientId == scheduledJob.JobEngineClientId).FirstOrDefault();
                var viewModelItem = Mapper.Map<ScheduledJob, ScheduledJobViewModel>(scheduledJob);
                viewModelItem.CustomerName = customer.Name;
                viewModelItem.JobEngineClientName = jobEngineClient.Name;
                viewModel.Add(viewModelItem);
            }
            return View(viewModel);
        }

        public ActionResult Create()
        {
            SelectJobTypeViewModel viewModel = new SelectJobTypeViewModel();
            viewModel.JobTypeItems = new List<JobTypeItem>();
            viewModel.JobTypeItems.Add(new JobTypeItem { Name = "Assembly Job", ActionLink = "SelectAssembly" });
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult SelectAssembly()
        {
            IAssemblyJobRepository repository = RepositoryFactory.GetAssemblyJobRepository();
            var automationJobs = repository.GetAll().ToList();
            var viewModel = Mapper.Map<List<AssemblyJob>, List<AssemblyJobViewModel>>(automationJobs);
            return View("SelectAssembly", viewModel);
        }

        [HttpGet]
        public ActionResult CreateAssemblyJob(int id)
        {
            ScheduledAssemblyJobViewModel viewModel = new ScheduledAssemblyJobViewModel();
            ICustomerRepository customerRepository = RepositoryFactory.GetCustomerRepository();

            var customers = customerRepository.GetAll();
            viewModel.Customers = new SelectList(customers, "CustomerId", "Name");

            IClientRepository clientRepository = RepositoryFactory.GetClientRespository();
            var jobEngineClients = clientRepository.GetAll();
            viewModel.JobEngineClients = new SelectList(jobEngineClients, "JobEngineClientId", "Name");

            IAssemblyJobRepository repository = RepositoryFactory.GetAssemblyJobRepository();
            var assemblyJob = repository.Get(id);
            viewModel.AssemblyJobId = id;
            viewModel.AssemblyJobParameters = Mapper.Map<List<AssemblyJobParameter>, List<AssemblyJobParameterViewModel>>(assemblyJob.Parameters);
            foreach (var param in viewModel.AssemblyJobParameters) { param.Value = ""; }
            return View(viewModel); 
        }
        
        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult CreateAssemblyJob(ScheduledAssemblyJobViewModel viewModel)
        {
            ICustomerRepository customerRepository = RepositoryFactory.GetCustomerRepository();
            var customers = customerRepository.GetAll();
            viewModel.Customers = new SelectList(customers, "CustomerId", "Name");

            IClientRepository clientRepository = RepositoryFactory.GetClientRespository();
            var jobEngineClients = clientRepository.GetAll();
            viewModel.JobEngineClients = new SelectList(jobEngineClients, "JobEngineClientId", "Name");

            if(ModelState.IsValid)
            {
                IAssemblyJobRepository repository = RepositoryFactory.GetAssemblyJobRepository();
                var assemblyJob = repository.Get(viewModel.AssemblyJobId);
                Dictionary<string, string> settings = new Dictionary<string, string>();
                int parametersErrorCount = 0;
                if (assemblyJob.Parameters != null)
                {
                    foreach (var param in assemblyJob.Parameters)
                    {
                        var viewModelParam = viewModel.AssemblyJobParameters.Where(x => x.Name == param.Name).FirstOrDefault();
                        if (param.IsRequired && viewModelParam.Value == null)
                        {
                            ModelState.AddModelError(param.Name, "Value is required");
                            parametersErrorCount++;
                            continue;
                        }
                        bool isValidParamValue = false;
                        int outInt;
                        long outLong;
                        switch (param.DataType)
                        {
                            case DataType.Int32: isValidParamValue = Int32.TryParse(viewModelParam.Value, out outInt);
                                break;
                            case DataType.Long: isValidParamValue = long.TryParse(viewModelParam.Value, out outLong);
                                break;
                            case DataType.String: isValidParamValue = true;
                                break;
                            default: isValidParamValue = true;
                                break;
                        }
                        if (!string.IsNullOrEmpty(param.InputValidationRegExPattern))
                        {
                            Regex regex = new Regex(param.InputValidationRegExPattern);
                            Match match = regex.Match(viewModelParam.Value);
                            if (match.Success)
                                isValidParamValue = true;
                            else
                                isValidParamValue = false;
                        }
                        if (!isValidParamValue)
                        {
                            ModelState.AddModelError(param.Name, "Parameter is invalid.  Please enter a valid value.");
                            parametersErrorCount++;
                            continue;
                        }

                        if (param.IsEncrypted)
                            settings.Add(param.Name, Encryption.Encrypt(viewModelParam.Value));
                        else
                            settings.Add(param.Name, viewModelParam.Value);
                    }
                }

                if(parametersErrorCount > 0)
                    return View(viewModel);
                
                assemblyJob.Settings = settings;
                assemblyJob.PluginFile = null;
                var serializedAssemblyJobSettings = JsonConvert.SerializeObject(assemblyJob);
                ScheduledJob scheduledJob = new ScheduledJob
                {
                    CreatedBy = User.Identity.Name,
                    CronSchedule = viewModel.CronSchedule,
                    CustomerId = viewModel.SelectedCustomerId,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow,
                    IsActive = viewModel.IsActive,
                    JobEngineClientId = viewModel.SelectedJobEngineClientId,
                    JobSettings = serializedAssemblyJobSettings,
                    JobType = JobType.AssemblyJob,
                    ModifiedBy = User.Identity.Name,
                    Name = viewModel.Name
                };
                IScheduledJobsRepository scheduledJobsRepository = RepositoryFactory.GetScheduledJobsRepository();
                int jobId = scheduledJobsRepository.CreateScheduledJob(scheduledJob);

                if(scheduledJob.IsActive)
                {
                    scheduledJob.ScheduledJobId = jobId;
                    IJobScheduler jobScheduler = new HangfireJobScheduler();
                    jobScheduler.AddOrUpdate(scheduledJob);                    
                }
                SuccessMessage = "Job has been created successfully.";
                return RedirectToAction("Index");
            }            
            return View(viewModel);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                IScheduledJobsRepository scheduledJobsRepository = RepositoryFactory.GetScheduledJobsRepository();
                var scheduledJob = scheduledJobsRepository.Get(id);
                scheduledJobsRepository.DeleteScheduledJob(id);
                IJobScheduler jobScheduler = new HangfireJobScheduler();
                jobScheduler.RemoveIfExists(scheduledJob.Name + "~" + scheduledJob.ScheduledJobId);
                SuccessMessage = "Job '" + scheduledJob.Name + "' has been deleted successfully.";                
            }
            catch (Exception e)
            {
                ErrorMessage = "An error occurred while delete the job.";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            IScheduledJobsRepository scheduledJobsRepository = RepositoryFactory.GetScheduledJobsRepository();
            var scheduledJob = scheduledJobsRepository.Get(id);
            switch (scheduledJob.JobType)
            {
                case JobType.AssemblyJob:
                    ScheduledAssemblyJobViewModel viewModel = new ScheduledAssemblyJobViewModel();                 
                    var assemblyJob = JsonConvert.DeserializeObject<AssemblyJob>(scheduledJob.JobSettings);
                    ICustomerRepository customerRepository = RepositoryFactory.GetCustomerRepository();
                    var customers = customerRepository.GetAll();
                    viewModel.Customers = new SelectList(customers, "CustomerId", "Name");
                    IClientRepository clientRepository = RepositoryFactory.GetClientRespository();
                    var jobEngineClients = clientRepository.GetAll();
                    viewModel.JobEngineClients = new SelectList(jobEngineClients, "JobEngineClientId", "Name");
                    viewModel.CronSchedule = scheduledJob.CronSchedule;
                    viewModel.IsActive = scheduledJob.IsActive;
                    viewModel.Name = scheduledJob.Name;
                    viewModel.SelectedCustomerId = scheduledJob.CustomerId;
                    viewModel.SelectedJobEngineClientId = scheduledJob.JobEngineClientId;
                    viewModel.ScheduledJobId = id;
                    IAssemblyJobRepository repository = RepositoryFactory.GetAssemblyJobRepository();
                    var assemblyJobDetails = repository.Get(assemblyJob.AssemblyJobId);
                    viewModel.AssemblyJobId = assemblyJobDetails.AssemblyJobId;
                    viewModel.AssemblyJobParameters = Mapper.Map<List<AssemblyJobParameter>, List<AssemblyJobParameterViewModel>>(assemblyJobDetails.Parameters);
                    foreach (var param in viewModel.AssemblyJobParameters) 
                    {
                        if (assemblyJob.Settings.ContainsKey(param.Name))
                        {
                            if (param.IsEncrypted)
                                param.Value = Encryption.Decrypt(assemblyJob.Settings[param.Name]);
                            else
                                param.Value = assemblyJob.Settings[param.Name];
                        }
                        else
                            param.Value = "";
                    }
                    return View("EditAssemblyJob", viewModel); 
                default:
                    break;
            }
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult EditAssemblyJob(ScheduledAssemblyJobViewModel viewModel)
        {
            ICustomerRepository customerRepository = RepositoryFactory.GetCustomerRepository();
            var customers = customerRepository.GetAll();
            viewModel.Customers = new SelectList(customers, "CustomerId", "Name");
            IClientRepository clientRepository = RepositoryFactory.GetClientRespository();
            var jobEngineClients = clientRepository.GetAll();
            viewModel.JobEngineClients = new SelectList(jobEngineClients, "JobEngineClientId", "Name");

            if (ModelState.IsValid)
            {
                IAssemblyJobRepository repository = RepositoryFactory.GetAssemblyJobRepository();
                var assemblyJob = repository.Get(viewModel.AssemblyJobId);
                IScheduledJobsRepository scheduledJobsRepository = RepositoryFactory.GetScheduledJobsRepository();
                var scheduledJob = scheduledJobsRepository.Get(viewModel.ScheduledJobId);
                Dictionary<string, string> settings = new Dictionary<string, string>();
                int parametersErrorCount = 0;
                foreach (var param in assemblyJob.Parameters)
                {
                    var viewModelParam = viewModel.AssemblyJobParameters.Where(x => x.Name == param.Name).FirstOrDefault();
                    if (param.IsRequired && viewModelParam.Value == null)
                    {
                        ModelState.AddModelError(param.Name, "Value is required");
                        parametersErrorCount++;
                        continue;
                    }
                    bool isValidParamValue = false;
                    int outInt;
                    long outLong;
                    switch (param.DataType)
                    {
                        case DataType.Int32: isValidParamValue = Int32.TryParse(viewModelParam.Value, out outInt);
                            break;
                        case DataType.Long: isValidParamValue = long.TryParse(viewModelParam.Value, out outLong);
                            break;
                        case DataType.String: isValidParamValue = true;
                            break;
                        default: isValidParamValue = true;
                            break;
                    }
                    if (!string.IsNullOrEmpty(param.InputValidationRegExPattern))
                    {
                        Regex regex = new Regex(param.InputValidationRegExPattern);
                        Match match = regex.Match(viewModelParam.Value);
                        if (match.Success)
                            isValidParamValue = true;
                        else
                            isValidParamValue = false;
                    }
                    if (!isValidParamValue)
                    {
                        ModelState.AddModelError(param.Name, "Parameter is invalid.  Please enter a valid value.");
                        parametersErrorCount++;
                        continue;
                    }

                    if (param.IsEncrypted)
                        settings.Add(param.Name, Encryption.Encrypt(viewModelParam.Value));
                    else
                        settings.Add(param.Name, viewModelParam.Value);
                }

                if (parametersErrorCount > 0)
                    return View(viewModel);

                assemblyJob.Settings = settings;
                assemblyJob.PluginFile = null;
                var serializedAssemblyJobSettings = JsonConvert.SerializeObject(assemblyJob);

                scheduledJob.CronSchedule = viewModel.CronSchedule;
                scheduledJob.JobEngineClientId = viewModel.SelectedJobEngineClientId;
                scheduledJob.CustomerId = viewModel.SelectedCustomerId;
                scheduledJob.DateModified = DateTime.UtcNow;
                scheduledJob.IsActive = viewModel.IsActive;
                scheduledJob.JobSettings = serializedAssemblyJobSettings;
                scheduledJob.ModifiedBy = User.Identity.Name;
                scheduledJob.Name = viewModel.Name;
                scheduledJobsRepository.UpdateScheduledJob(scheduledJob);

                IJobScheduler jobScheduler = new HangfireJobScheduler();
                jobScheduler.RemoveIfExists(scheduledJob.Name + "~" + scheduledJob.ScheduledJobId);

                if(scheduledJob.IsActive)
                    jobScheduler.AddOrUpdate(scheduledJob);
               

                SuccessMessage = "Job has been saved successfully.";
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public ActionResult Details(int id)
        {
            IScheduledJobsRepository scheduledJobsRepository = RepositoryFactory.GetScheduledJobsRepository();
            var scheduledJob = scheduledJobsRepository.Get(id);
            switch (scheduledJob.JobType)
            {
                case JobType.AssemblyJob:
                    ScheduledAssemblyJobViewModel viewModel = new ScheduledAssemblyJobViewModel();
                    var assemblyJob = JsonConvert.DeserializeObject<AssemblyJob>(scheduledJob.JobSettings);
                    ICustomerRepository customerRepository = RepositoryFactory.GetCustomerRepository();
                    var customers = customerRepository.GetAll();
                    viewModel.Customers = new SelectList(customers, "CustomerId", "Name");
                    IClientRepository clientRepository = RepositoryFactory.GetClientRespository();
                    var jobEngineClients = clientRepository.GetAll();
                    viewModel.JobEngineClients = new SelectList(jobEngineClients, "JobEngineClientId", "Name");
                    viewModel.CronSchedule = scheduledJob.CronSchedule;
                    viewModel.IsActive = scheduledJob.IsActive;
                    viewModel.Name = scheduledJob.Name;
                    viewModel.SelectedCustomerId = scheduledJob.CustomerId;
                    viewModel.SelectedJobEngineClientId = scheduledJob.JobEngineClientId;
                    viewModel.ScheduledJobId = id;
                    IAssemblyJobRepository repository = RepositoryFactory.GetAssemblyJobRepository();
                    var assemblyJobDetails = repository.Get(assemblyJob.AssemblyJobId);
                    viewModel.AssemblyJobId = assemblyJobDetails.AssemblyJobId;
                    viewModel.AssemblyJobParameters = Mapper.Map<List<AssemblyJobParameter>, List<AssemblyJobParameterViewModel>>(assemblyJobDetails.Parameters);
                    foreach (var param in viewModel.AssemblyJobParameters)
                    {
                        if (assemblyJob.Settings.ContainsKey(param.Name))
                        {
                            if (param.IsEncrypted)
                                param.Value = Encryption.Decrypt(assemblyJob.Settings[param.Name]);
                            else
                                param.Value = assemblyJob.Settings[param.Name];
                        }
                        else
                            param.Value = "";
                    }
                    return View("DetailsAssemblyJob", viewModel);
                default:
                    break;
            }
            return RedirectToAction("Index");
        }

        public ActionResult TriggerNow(int id)
        {
            IScheduledJobsRepository scheduledJobsRepository = RepositoryFactory.GetScheduledJobsRepository();
            var scheduledJob = scheduledJobsRepository.Get(id);
            JobEngine.Persistence.JobQueue jobQueue = new Persistence.JobQueue();
            long jobExecutionQueueId = jobQueue.QueueScheduledJob(scheduledJob);
            IJobExecutionQueueRepository jobExecutionQueueRepository = RepositoryFactory.GetJobExecutionQueueRepository();
            var jobExecutionQueueItem = jobExecutionQueueRepository.Get(jobExecutionQueueId);
            TriggerNowJobResultsViewModel viewModel = Mapper.Map<JobExecutionQueue, TriggerNowJobResultsViewModel>(jobExecutionQueueItem);
            ViewBag.JobTitle = scheduledJob.Name;
            return View(viewModel);
        }

        public ActionResult GetJobExecutionQueueStatus(long jobExecutionQueueId)
        {
            IJobExecutionQueueRepository repo = RepositoryFactory.GetJobExecutionQueueRepository();
            var queueItem = repo.Get(jobExecutionQueueId);
            dynamic dynamicObject = new ExpandoObject();
            dynamicObject.Status = queueItem.JobExecutionStatus.ToString();
            dynamicObject.ClientPickup = queueItem.DateReceivedByClient.HasValue ? queueItem.DateReceivedByClient : null;
            dynamicObject.TotalExecutionTime = queueItem.TotalExecutionTimeInMs.HasValue ? queueItem.TotalExecutionTimeInMs : null;
            ILoggingRepository logsRepository = RepositoryFactory.GetLoggingRepository();            
            var logs = logsRepository.GetLogs(jobExecutionQueueId);
            dynamicObject.Logs = logs;
            var serailizedJsonResult = JsonConvert.SerializeObject(dynamicObject);
            return Content(serailizedJsonResult, "application/json");
        }

        
    }
}