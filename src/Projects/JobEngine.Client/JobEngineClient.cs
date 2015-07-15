using AssemblyJobHelper;
using JobEngine.Models;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace JobEngine.Client
{
    public class JobEngineClient
    {
        private BlockingCollection<JobExecutionQueue> jobQueue = new BlockingCollection<JobExecutionQueue>();
        private CancellationTokenSource consumerCancelTokenSource = new CancellationTokenSource();
        private bool isStopping = false;
        private bool isCurrentlyDownloadingJobs = false;
        private static readonly ILog log = LogManager.GetLogger(typeof(JobEngineClient));
        private static string logger = typeof(JobEngineClient).Name;
        private Thread jobsConsumerThread;
        private Thread jobsProducerThread;
        private Thread realTimeListenerThread;
        private bool isSynchingAssemblyJobs = false;
        private System.Timers.Timer synchAssembliesTimer;
        private RealTimeServerConnection realTimeConnection;

        public JobEngineClient()
        {
            XmlConfigurator.Configure();
        }

        public void Start()
        {
            InitializeSyncAssemblyJobsTimer();

            jobsProducerThread = new Thread(ProduceJobs);
            jobsProducerThread.Start();

            jobsConsumerThread = new Thread(ConsumeJobs);
            jobsConsumerThread.Start();

            realTimeListenerThread = new Thread(ConnectToRealTimeServer);
            realTimeListenerThread.Start();
        }

        private void ConnectToRealTimeServer()
        {
            try
            {
                realTimeConnection = new RealTimeServerConnection();
                realTimeConnection.PollRequested += RealTimeConnection_PollRequested;
                realTimeConnection.Connect(Settings.RealTimeUrl, Settings.RealTimeHubName, Settings.JobEngineClientId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void RealTimeConnection_PollRequested(object sender, EventArgs e)
        {
            DownloadJobsWaitingToExecute();
        }

        private void InitializeSyncAssemblyJobsTimer()
        {
            synchAssembliesTimer = new System.Timers.Timer(300 * 1000);
            synchAssembliesTimer.Elapsed += SynchAssembliesTimer_Elapsed;
            SynchAssemblies();
            synchAssembliesTimer.Start();
        }

        public void Stop()
        {
            isStopping = true;
            consumerCancelTokenSource.Cancel();
            synchAssembliesTimer.Stop();
            synchAssembliesTimer.Dispose();
            realTimeConnection.Disconnect();
        }

        public void ProduceJobs()
        {
            while (!isStopping)
            {
                DownloadJobsWaitingToExecute();
                Thread.Sleep(Settings.PollInterval * 1000);
            }
        }

        private void DownloadJobsWaitingToExecute()
        {
            if (!isCurrentlyDownloadingJobs)
            {
                isCurrentlyDownloadingJobs = true;
                try
                {
                    JobEngineApi api = new JobEngineApi(Settings.ApiUrl, Settings.ApiUsername, Settings.ApiPassword);
                    var jobs = api.GetJobsWaitingToExecute(Settings.JobEngineClientId, ClientInfoHelper.GetLocalHostIPAddress(), Environment.MachineName).Result;
                    foreach (var job in jobs)
                    {
                        jobQueue.Add(job);
                        api.AckJobRecieved(job.JobExecutionQueueId, DateTime.UtcNow).Wait();
                    }
                }
                catch (AggregateException e)
                {
                    foreach (var exception in e.InnerExceptions)
                    {
                        log.Error(exception);
                    }
                }
                isCurrentlyDownloadingJobs = false;
            }
        }

        public void ConsumeJobs()
        {
            foreach (var jobQueueItem in jobQueue.GetConsumingEnumerable(consumerCancelTokenSource.Token))
            {
                switch (jobQueueItem.JobType)
                {
                    case JobType.AssemblyJob:
                        Task.Factory.StartNew(() => ProcessAssemblyJob(jobQueueItem), TaskCreationOptions.LongRunning);
                        break;
                    case JobType.PowerShell:
                        Task.Factory.StartNew(() => ProcessPowerShellJob(jobQueueItem), TaskCreationOptions.LongRunning);
                        break;
                    default:
                        break;
                }
            }
        }

        private async void ProcessPowerShellJob(JobExecutionQueue jobQueueItem)
        {
            JobEngineApi jobEngineApi = new JobEngineApi(Settings.ApiUrl, Settings.ApiUsername, Settings.ApiPassword);

            try
            {
                await jobEngineApi.AddJobExecutionLogEntry(jobQueueItem.JobExecutionQueueId,
                                date: DateTime.UtcNow,
                                logLevel: JobEngine.Models.LogLevel.INFO,
                                logger: logger,
                                message: "Beginning to execute job",
                                exception: null);

                await jobEngineApi.UpdateJobExecutionStatus(jobQueueItem.JobExecutionQueueId, JobExecutionStatus.EXECUTING);

                DateTime startTime = DateTime.UtcNow;
                var powerShellJob = JsonConvert.DeserializeObject<PowerShellJob>(jobQueueItem.JobSettings);

                PowerShellJobExecutor executor = new PowerShellJobExecutor();
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                PowerShellJobResult result = executor.Execute(jobExecutionQueueId: jobQueueItem.JobExecutionQueueId, powerShellJob: powerShellJob);
                stopWatch.Stop();
                result.DateCompleted = DateTime.UtcNow;
                result.ScheduledJobId = jobQueueItem.ScheduledJobId;
                result.JobExecutionQueueId = jobQueueItem.JobExecutionQueueId;

                await jobEngineApi.CreatePowerShellJobResult(result);

                string errors = GetErrorString(result.Errors);

                await jobEngineApi.AddJobExecutionLogEntry(jobQueueItem.JobExecutionQueueId,
                                date: DateTime.UtcNow,
                                logLevel: (result.Errors.Count > 0) ? JobEngine.Models.LogLevel.ERROR : JobEngine.Models.LogLevel.INFO,
                                logger: logger,
                                exception: null,
                                message: !string.IsNullOrEmpty(errors) ? "Finished executing PowerShell Job with " + result.Errors.Count + " errors reported\r\n" + errors :
                                                                         "Finished executing PowerShell Job with " + result.Errors.Count + " errors reported");
                                

                await jobEngineApi.UpdateJobExecutionResult(jobQueueItem.JobExecutionQueueId,
                                jobExecutionStatus: StatusMapper.FromAssemblyJobResultToJobExecutionResult(Result.FATAL),
                                resultMessage: (result.Errors.Count > 0) ? result.Errors.Count + " error/s reported\r\n" + errors : "Finished Executing Successfully",
                                dateCompleted: DateTime.UtcNow,
                                totalExecutionTimeInMs: stopWatch.ElapsedMilliseconds);

            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        private async void ProcessAssemblyJob(JobExecutionQueue jobQueueItem)
        {
            JobEngineApi jobEngineApi = new JobEngineApi(Settings.ApiUrl, Settings.ApiUsername, Settings.ApiPassword);

            try
            {
                await jobEngineApi.AddJobExecutionLogEntry(jobQueueItem.JobExecutionQueueId,
                                date: DateTime.UtcNow,
                                logLevel: JobEngine.Models.LogLevel.INFO,
                                logger: logger,
                                message: "Beginning to execute job",
                                exception: null);

                await jobEngineApi.UpdateJobExecutionStatus(jobQueueItem.JobExecutionQueueId, JobExecutionStatus.EXECUTING);

                DateTime startTime = DateTime.UtcNow;
                var assemblyJob = JsonConvert.DeserializeObject<AssemblyJob>(jobQueueItem.JobSettings);

                AssemblyJobProcessor assemblyJobProcessor = new AssemblyJobProcessor();
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                AssemblyJobResult result = assemblyJobProcessor.ProcessJob(
                                                        jobExecutionQueueId: jobQueueItem.JobExecutionQueueId,
                                                        assemblyJob: assemblyJob,
                                                        logger: new AssemblyJobLogger());
                stopWatch.Stop();

                await jobEngineApi.AddJobExecutionLogEntry(jobQueueItem.JobExecutionQueueId,
                                date: DateTime.UtcNow,
                                logLevel: JobEngine.Models.LogLevel.INFO,
                                logger: logger,
                                message: "Finished executing job",
                                exception: null);

                await jobEngineApi.UpdateJobExecutionResult(jobQueueItem.JobExecutionQueueId,
                                jobExecutionStatus: StatusMapper.FromAssemblyJobResultToJobExecutionResult(result.Result),
                                resultMessage: result.Message,
                                dateCompleted: DateTime.UtcNow,
                                totalExecutionTimeInMs: stopWatch.ElapsedMilliseconds);
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        private void SynchAssembliesTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SynchAssemblies();
        }

        private void SynchAssemblies()
        {
            if (!isSynchingAssemblyJobs)
            {
                log.Debug("Synching Assembly Jobs");
                isSynchingAssemblyJobs = true;
                try
                {
                    JobEngineApi api = new JobEngineApi(Settings.ApiUrl, Settings.ApiUsername, Settings.ApiPassword);
                    string zipFileName = api.SynchronizeAssemblies(Settings.AssemblyJobPluginsDirectory);
                    if (!string.IsNullOrEmpty(zipFileName))
                    {
                        string destination = zipFileName.Replace(".zip", "\\");
                        Directory.CreateDirectory(destination);
                        ZipArchive zipArchive = ZipFile.OpenRead(zipFileName);
                        foreach (ZipArchiveEntry entry in zipArchive.Entries)
                        {
                            log.Debug("Extracting updated Assembly Job Plugin: " + entry.Name);
                            entry.ExtractToFile(Path.Combine(Settings.AssemblyJobPluginsDirectory, entry.Name), true);
                            log.Debug("Finished extracting updated Assembly Job Plugin: " + entry.Name);
                        }

                        Task.Factory.StartNew(() =>
                        {
                            Thread.Sleep(60000); // give clr time to release the file lock
                            File.Delete(zipFileName);
                        });
                    }
                }
                catch (Exception e)
                {
                    log.Warn(e);
                }
                isSynchingAssemblyJobs = false;
                log.Debug("Finished Synching Assembly Jobs");
            }
        }

        private string GetErrorString(List<ErrorInfo> errors)
        {
            string result = string.Empty;
            if (errors.Count > 0)
            {
                var errorsList = errors.Select(x => "Message: " + x.Message + "\r\nStack Trace: " + x.Exception.StackTrace + "\r\n").ToList();
                result = string.Join("\r\n\r\n", errorsList);
            }
            return result;
        }
    }
}
