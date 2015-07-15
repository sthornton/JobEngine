using AutoMapper;
using JobEngine.Common;
using JobEngine.Core.Persistence;
using JobEngine.Models;
using JobEngine.Web.Areas.AssemblyJobs.Models;
using JobEngine.Web.Areas.PowerShellJobs.Models;
using JobEngine.Web.Areas.ScheduledJobs.Models;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JobEngine.Web.Areas.ScheduledJobs.Controllers
{
    public class HomeController : BaseController
    {
        private ICustomerRepository customerRepository;
        private IScheduledJobsRepository scheduledJobsRepository;
        private IClientRepository clientRepository;
        private IAssemblyJobRepository assemblyJobsRepository;
        private IJobScheduler jobScheduler;
        private ILoggingRepository loggingRepository;
        private IJobExecutionQueueRepository jobExecutionQueueRepository;
        private IPowerShellJobsRepository powerShellJobsRepository;

        public HomeController(ICustomerRepository customerRepository,
                              IScheduledJobsRepository scheduledJobsRepository,
                              IClientRepository clientRepository,
                              IAssemblyJobRepository assemblyJobsRepository,
                              IPowerShellJobsRepository powerShellJobsRepository,
                              IJobScheduler jobScheduler,
                              ILoggingRepository loggingRepository,
                              IJobExecutionQueueRepository jobExecutionQueueRepository)
        {
            this.customerRepository = customerRepository;
            this.scheduledJobsRepository = scheduledJobsRepository;
            this.clientRepository = clientRepository;
            this.assemblyJobsRepository = assemblyJobsRepository;
            this.jobScheduler = jobScheduler;
            this.loggingRepository = loggingRepository;
            this.jobExecutionQueueRepository = jobExecutionQueueRepository;
            this.powerShellJobsRepository = powerShellJobsRepository;
        }

        public async Task<ActionResult> Index()
        {
            var customers = this.customerRepository.GetAll();
            var scheduledJobs = await this.scheduledJobsRepository.GetAll();
            var clients = await this.clientRepository.GetAllAsync();

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
            viewModel.JobTypeItems.Add(new JobTypeItem { Name = "PowerShell", ActionLink = "SelectPowerShellScript" });
            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> SelectAssembly()
        {
            var assemblyJobs = await this.assemblyJobsRepository.GetAllAsync();
            var viewModel = Mapper.Map<List<AssemblyJob>, List<AssemblyJobViewModel>>(assemblyJobs.ToList());
            return View("SelectAssembly", viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> SelectPowerShellScript()
        {
            var powerShellScripts = await this.powerShellJobsRepository.GetAllAsync();
            var viewModel = Mapper.Map<List<PowerShellJob>, List<PowerShellJobViewModel>>(powerShellScripts.ToList());
            return View("SelectPowerShellScript", viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> CreatePowerShellJob(int id)
        {
            ScheduledPowerShellJobViewModel viewModel = new ScheduledPowerShellJobViewModel();
            var customers = this.customerRepository.GetAll();
            viewModel.Customers = new SelectList(customers, "CustomerId", "Name");
            var jobEngineClients = await this.clientRepository.GetAllAsync();
            viewModel.JobEngineClients = new SelectList(jobEngineClients, "JobEngineClientId", "Name");

            var powerShellJob = await this.powerShellJobsRepository.GetAsync(id);
            viewModel.PowerShellJobId = id;
            viewModel.PowerShellJobParameters = Mapper.Map<List<PowerShellJobParameter>, List<PowerShellJobParameterViewModel>>(powerShellJob.Parameters);
            foreach (var item in viewModel.PowerShellJobParameters) { item.Value = ""; }
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> CreatePowerShellJob(ScheduledPowerShellJobViewModel viewModel)
        {
            var customers = this.customerRepository.GetAll();
            viewModel.Customers = new SelectList(customers, "CustomerId", "Name");

            var jobEngineClients = await this.clientRepository.GetAllAsync();
            viewModel.JobEngineClients = new SelectList(jobEngineClients, "JobEngineClientId", "Name");

            if (ModelState.IsValid)
            {
                var powerShellJob = await this.powerShellJobsRepository.GetAsync(viewModel.PowerShellJobId);
                Dictionary<string, string> settings = new Dictionary<string, string>();
                int parametersErrorCount = 0;
                if (powerShellJob.Parameters != null)
                {
                    foreach (var param in powerShellJob.Parameters)
                    {
                        var viewModelParam = viewModel.PowerShellJobParameters.Where(x => x.Name == param.Name).FirstOrDefault();
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
                            param.Value = Encryption.Encrypt(viewModelParam.Value);
                        else
                            param.Value = viewModelParam.Value;
                    }
                }

                if (parametersErrorCount > 0)
                    return View(viewModel);

                var serializedPowerShellJobSettings = JsonConvert.SerializeObject(powerShellJob);
                ScheduledJob scheduledJob = new ScheduledJob
                {
                    CreatedBy = User.Identity.Name,
                    CronSchedule = viewModel.CronSchedule,
                    CustomerId = viewModel.SelectedCustomerId,
                    DateCreated = DateTime.UtcNow,
                    DateModified = DateTime.UtcNow,
                    IsActive = viewModel.IsActive,
                    JobEngineClientId = viewModel.SelectedJobEngineClientId,
                    JobSettings = serializedPowerShellJobSettings,
                    JobType = JobType.PowerShell,
                    ModifiedBy = User.Identity.Name,
                    Name = viewModel.Name
                };
                int jobId = await this.scheduledJobsRepository.CreateScheduledJob(scheduledJob);

                if (scheduledJob.IsActive)
                {
                    scheduledJob.ScheduledJobId = jobId;
                    this.jobScheduler.AddOrUpdate(scheduledJob);
                }
                SuccessMessage = "Job has been created successfully.";
                return RedirectToAction("Index");
            }          
            return View(viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> CreateAssemblyJob(int id)
        {
            ScheduledAssemblyJobViewModel viewModel = new ScheduledAssemblyJobViewModel();

            var customers = this.customerRepository.GetAll();
            viewModel.Customers = new SelectList(customers, "CustomerId", "Name");

            var jobEngineClients = await this.clientRepository.GetAllAsync();
            viewModel.JobEngineClients = new SelectList(jobEngineClients, "JobEngineClientId", "Name");

            var assemblyJob = await this.assemblyJobsRepository.GetAsync(id);
            viewModel.AssemblyJobId = id;
            viewModel.AssemblyJobParameters = Mapper.Map<List<AssemblyJobParameter>, List<AssemblyJobParameterViewModel>>(assemblyJob.Parameters);
            foreach (var param in viewModel.AssemblyJobParameters) { param.Value = ""; }
            return View(viewModel); 
        }
        
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAssemblyJob(ScheduledAssemblyJobViewModel viewModel)
        {
            var customers = this.customerRepository.GetAll();
            viewModel.Customers = new SelectList(customers, "CustomerId", "Name");

            var jobEngineClients = await this.clientRepository.GetAllAsync();
            viewModel.JobEngineClients = new SelectList(jobEngineClients, "JobEngineClientId", "Name");

            if(ModelState.IsValid)
            {
                var assemblyJob = await this.assemblyJobsRepository.GetAsync(viewModel.AssemblyJobId);
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
                int jobId = await this.scheduledJobsRepository.CreateScheduledJob(scheduledJob);

                if(scheduledJob.IsActive)
                {
                    scheduledJob.ScheduledJobId = jobId;
                    this.jobScheduler.AddOrUpdate(scheduledJob);                    
                }
                SuccessMessage = "Job has been created successfully.";
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var scheduledJob = await this.scheduledJobsRepository.Get(id);
                await this.scheduledJobsRepository.DeleteScheduledJob(id);
                this.jobScheduler.RemoveIfExists(scheduledJob.Name + "~" + scheduledJob.ScheduledJobId);
                SuccessMessage = "Job '" + scheduledJob.Name + "' has been deleted successfully.";                
            }
            catch (Exception e)
            {
                ErrorMessage = "An error occurred while deleting the job.";
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int id)
        {
            var scheduledJob =await this.scheduledJobsRepository.Get(id);
            var customers = this.customerRepository.GetAll();
            SelectList customerSelectList = new SelectList(customers, "CustomerId", "Name");
            var jobEngineClients = await this.clientRepository.GetAllAsync();
            SelectList jobEngineClientsSelectList = new SelectList(jobEngineClients, "JobEngineClientId", "Name");

            switch (scheduledJob.JobType)
            {
                case JobType.AssemblyJob:
                    ScheduledAssemblyJobViewModel viewModel = new ScheduledAssemblyJobViewModel();                 
                    var assemblyJob = JsonConvert.DeserializeObject<AssemblyJob>(scheduledJob.JobSettings);
                    viewModel.Customers = customerSelectList;
                    viewModel.JobEngineClients = jobEngineClientsSelectList;
                    viewModel.CronSchedule = scheduledJob.CronSchedule;
                    viewModel.IsActive = scheduledJob.IsActive;
                    viewModel.Name = scheduledJob.Name;
                    viewModel.SelectedCustomerId = scheduledJob.CustomerId;
                    viewModel.SelectedJobEngineClientId = scheduledJob.JobEngineClientId;
                    viewModel.ScheduledJobId = id;
                    var assemblyJobDetails = await this.assemblyJobsRepository.GetAsync(assemblyJob.AssemblyJobId);
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
                case JobType.PowerShell :
                    ScheduledPowerShellJobViewModel scheduledPowerShellJobViewModel = new ScheduledPowerShellJobViewModel();
                    var powerShellJob = JsonConvert.DeserializeObject<PowerShellJob>(scheduledJob.JobSettings);
                    scheduledPowerShellJobViewModel.Customers = customerSelectList;
                    scheduledPowerShellJobViewModel.JobEngineClients = jobEngineClientsSelectList;
                    scheduledPowerShellJobViewModel.CronSchedule = scheduledJob.CronSchedule;
                    scheduledPowerShellJobViewModel.IsActive = scheduledJob.IsActive;
                    scheduledPowerShellJobViewModel.Name = scheduledJob.Name;
                    scheduledPowerShellJobViewModel.PowerShellJobId = powerShellJob.PowerShellJobId;
                    scheduledPowerShellJobViewModel.PowerShellJobParameters = Mapper.Map<List<PowerShellJobParameter>, List<PowerShellJobParameterViewModel>>(powerShellJob.Parameters);
                    scheduledPowerShellJobViewModel.ScheduledJobId = scheduledJob.ScheduledJobId;
                    scheduledPowerShellJobViewModel.SelectedCustomerId = scheduledJob.CustomerId;
                    scheduledPowerShellJobViewModel.SelectedJobEngineClientId = scheduledJob.JobEngineClientId;
                    foreach (var param in scheduledPowerShellJobViewModel.PowerShellJobParameters)
                    {
                            if (param.IsEncrypted)
                                param.Value = Encryption.Decrypt(param.Value);                                                 
                    }
                    return View("EditPowerShellJob", scheduledPowerShellJobViewModel);
                default:
                    break;
            }
            return RedirectToAction("Index");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPowerShellJob(ScheduledPowerShellJobViewModel viewModel)
        {
            var customers = this.customerRepository.GetAll();
            viewModel.Customers = new SelectList(customers, "CustomerId", "Name");
            var jobEngineClients = await this.clientRepository.GetAllAsync();
            viewModel.JobEngineClients = new SelectList(jobEngineClients, "JobEngineClientId", "Name");

            if (ModelState.IsValid)
            {
                var powerShellJob = await this.powerShellJobsRepository.GetAsync(viewModel.PowerShellJobId);
                var scheduledJob = await this.scheduledJobsRepository.Get(viewModel.ScheduledJobId);
                Dictionary<string, string> settings = new Dictionary<string, string>();
                int parametersErrorCount = 0;
                foreach (var param in powerShellJob.Parameters)
                {
                    var viewModelParam = viewModel.PowerShellJobParameters.Where(x => x.Name == param.Name).FirstOrDefault();
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
                        param.Value = Encryption.Encrypt(viewModelParam.Value);
                    else
                        param.Value = viewModelParam.Value;
                }

                if (parametersErrorCount > 0)
                    return View(viewModel);

                var serializedPowerShellJobSettings = JsonConvert.SerializeObject(powerShellJob);
                scheduledJob.CronSchedule = viewModel.CronSchedule;
                scheduledJob.JobEngineClientId = viewModel.SelectedJobEngineClientId;
                scheduledJob.CustomerId = viewModel.SelectedCustomerId;
                scheduledJob.DateModified = DateTime.UtcNow;
                scheduledJob.IsActive = viewModel.IsActive;
                scheduledJob.JobSettings = serializedPowerShellJobSettings;
                scheduledJob.ModifiedBy = User.Identity.Name;
                scheduledJob.Name = viewModel.Name;
                await this.scheduledJobsRepository.UpdateScheduledJob(scheduledJob);

                this.jobScheduler.RemoveIfExists(scheduledJob.Name + "~" + scheduledJob.ScheduledJobId);

                if (scheduledJob.IsActive)
                    this.jobScheduler.AddOrUpdate(scheduledJob);

                SuccessMessage = "Job has been saved successfully.";
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAssemblyJob(ScheduledAssemblyJobViewModel viewModel)
        {
            var customers = this.customerRepository.GetAll();
            viewModel.Customers = new SelectList(customers, "CustomerId", "Name");
            var jobEngineClients = await this.clientRepository.GetAllAsync();
            viewModel.JobEngineClients = new SelectList(jobEngineClients, "JobEngineClientId", "Name");

            if (ModelState.IsValid)
            {
                var assemblyJob = await this.assemblyJobsRepository.GetAsync(viewModel.AssemblyJobId);
                var scheduledJob = await this.scheduledJobsRepository.Get(viewModel.ScheduledJobId);
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
                await this.scheduledJobsRepository.UpdateScheduledJob(scheduledJob);

                this.jobScheduler.RemoveIfExists(scheduledJob.Name + "~" + scheduledJob.ScheduledJobId);

                if(scheduledJob.IsActive)
                    this.jobScheduler.AddOrUpdate(scheduledJob);
               

                SuccessMessage = "Job has been saved successfully.";
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public async Task<ActionResult> Details(int id)
        {
            var scheduledJob = await this.scheduledJobsRepository.Get(id);
            switch (scheduledJob.JobType)
            {
                case JobType.AssemblyJob:
                    ScheduledAssemblyJobViewModel viewModel = new ScheduledAssemblyJobViewModel();
                    var assemblyJob = JsonConvert.DeserializeObject<AssemblyJob>(scheduledJob.JobSettings);
                    var customers = this.customerRepository.GetAll();
                    viewModel.Customers = new SelectList(customers, "CustomerId", "Name");
                    var jobEngineClients = await this.clientRepository.GetAllAsync();
                    viewModel.JobEngineClients = new SelectList(jobEngineClients, "JobEngineClientId", "Name");
                    viewModel.CronSchedule = scheduledJob.CronSchedule;
                    viewModel.IsActive = scheduledJob.IsActive;
                    viewModel.Name = scheduledJob.Name;
                    viewModel.SelectedCustomerId = scheduledJob.CustomerId;
                    viewModel.SelectedJobEngineClientId = scheduledJob.JobEngineClientId;
                    viewModel.ScheduledJobId = id;
                    var assemblyJobDetails = await this.assemblyJobsRepository.GetAsync(assemblyJob.AssemblyJobId);
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

        public async Task<ActionResult> TriggerNow(int id)
        {
            var scheduledJob = await this.scheduledJobsRepository.Get(id);
            JobEngine.Persistence.JobQueue jobQueue = new Persistence.JobQueue();
            long jobExecutionQueueId = jobQueue.QueueScheduledJob(scheduledJob);
            ClientCommunicator clientCommunicator = new ClientCommunicator();
            clientCommunicator.SendPollRequest(scheduledJob.JobEngineClientId);
            var jobExecutionQueueItem = await this.jobExecutionQueueRepository.GetAsync(jobExecutionQueueId);
            TriggerNowJobResultsViewModel viewModel = Mapper.Map<JobExecutionQueue, TriggerNowJobResultsViewModel>(jobExecutionQueueItem);
            ViewBag.JobTitle = scheduledJob.Name;
            return View(viewModel);
        }

        public async Task<ActionResult> GetJobExecutionQueueStatus(long jobExecutionQueueId)
        {
            var queueItem = await this.jobExecutionQueueRepository.GetAsync(jobExecutionQueueId);
            dynamic dynamicObject = new ExpandoObject();
            dynamicObject.Status = queueItem.JobExecutionStatus.ToString();
            dynamicObject.ClientPickup = queueItem.DateReceivedByClient.HasValue ? queueItem.DateReceivedByClient : null;
            dynamicObject.TotalExecutionTime = queueItem.TotalExecutionTimeInMs.HasValue ? queueItem.TotalExecutionTimeInMs : null;            
            var logs = await this.loggingRepository.GetLogs(jobExecutionQueueId);
            dynamicObject.Logs = logs;
            var serailizedJsonResult = JsonConvert.SerializeObject(dynamicObject);
            return Content(serailizedJsonResult, "application/json");
        }

        public async Task<ActionResult> ViewHistory(int id)
        {
            var scheduledJob = await this.scheduledJobsRepository.Get(id);
            var queueItems = await this.jobExecutionQueueRepository.GetScheduledJobsAsync(id);
            ViewHistoryViewModel viewModel = new ViewHistoryViewModel();
            viewModel.ScheduledJob = Mapper.Map<ScheduledJob, ScheduledJobViewModel>(scheduledJob);
            viewModel.JobExecutionQueueItems = queueItems;
            return View(viewModel);
        }

        public async Task<ActionResult> ViewExecutionDetails(int id)
        {
            ExecutionDetailsViewModel viewModel = new ExecutionDetailsViewModel();
            var jobQueueItem = await this.jobExecutionQueueRepository.GetAsync(id);
            var scheduledJob = await this.scheduledJobsRepository.Get(jobQueueItem.ScheduledJobId.Value);
            viewModel.ScheduledJobId = scheduledJob.ScheduledJobId;
            viewModel.ScheduledJobName = scheduledJob.Name;
            viewModel.StringResults = jobQueueItem.ResultMessage;

            switch (scheduledJob.JobType)
            {
                case JobType.AssemblyJob:
                    viewModel.StringResults = jobQueueItem.ResultMessage;
                    break;
                case JobType.PowerShell:
                    PowerShellJob psJob = JsonConvert.DeserializeObject<PowerShellJob>(scheduledJob.JobSettings);
                    var psResult = await this.powerShellJobsRepository.GetPowerShellResultFromExecutionId(id);
                    if (psResult == null || psResult.Results == null)
                    {
                        ErrorMessage = "Results not received yet!";
                        return RedirectToAction("ViewHistory", new { id = scheduledJob.ScheduledJobId });
                    }
                    if(psJob.PSResultType == PSResultType.Table && psResult != null)
                        viewModel.DataTableResults = JsonConvert.DeserializeObject<DataTable>(psResult.Results, new DataTableConverter());
                    else
                        viewModel.StringResults = psResult.Results;                        
                    break;
                default:
                    break;
            }
            return View(viewModel);
        }
    }
}