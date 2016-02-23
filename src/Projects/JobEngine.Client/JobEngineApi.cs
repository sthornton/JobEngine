using JobEngine.Common;
using JobEngine.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace JobEngine.Client
{
    public class JobEngineApi
    {
        private string m_Username;
        private string m_Password;
        private Uri m_BaseUri;

        public JobEngineApi(string baseAddress, string username, string password)
        {
            m_BaseUri = new Uri(baseAddress);
            m_Username = username;
            m_Password = password;
        }

        public async Task<IEnumerable<Customer>> GetCustomerById(Guid id)
        {
            IEnumerable<Customer> results;
            HttpResponseMessage responseMessage;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetAccessToken());
                responseMessage = await client.GetAsync(new Uri(m_BaseUri, "api/JobEngineClient/GetAllClients/"));
                responseMessage.EnsureSuccessStatusCode();
                results = await responseMessage.Content.ReadAsAsync<IEnumerable<Customer>>();
            }
            return results;
        }

        public string SynchronizeAssemblies(string directory)
        {
            string resultZipFileName = string.Empty;            
            string[] files = Directory.GetFiles(directory,"*.dll");
            Dictionary<string, DateTime> fileDetails = new Dictionary<string, DateTime>();
            foreach(string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                fileDetails.Add(fileInfo.Name, fileInfo.LastWriteTimeUtc);
            }
            HttpResponseMessage responseMessage;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetAccessToken());
                responseMessage = client.PostAsync(new Uri(m_BaseUri, "api/FilesHandler/SynchAssemblies?jobEngineClientId=" + Settings.JobEngineClientId.ToString()), fileDetails, new JsonMediaTypeFormatter()).Result;
                responseMessage.EnsureSuccessStatusCode();
                var byteArray = responseMessage.Content.ReadAsByteArrayAsync().Result;
                if (byteArray.Length > 0)
                {
                    File.WriteAllBytes(
                            path: Path.Combine(Settings.TempFileDirectory, responseMessage.Content.Headers.ContentDisposition.FileName),
                            bytes: byteArray);
                            resultZipFileName = Path.Combine(Settings.TempFileDirectory, responseMessage.Content.Headers.ContentDisposition.FileName);
                }

            }
            return resultZipFileName;
        }

        public async Task AckJobRecieved(long jobExecutionQueueId, DateTime dateRecieved)
        {
            try
            {
                HttpResponseMessage responseMessage;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetAccessToken());
                    responseMessage = await client.PostAsync(new Uri(m_BaseUri, "api/JobExecutionQueue/AckJobRecieved?jobExecutionQueueId=" + jobExecutionQueueId.ToString() + "&dateRecieved=" + dateRecieved.ToString()), null);
                    responseMessage.EnsureSuccessStatusCode();
                }
            }
            catch (Exception)
            {                
                throw;
            }
        }

        public async Task UpdateJobExecutionStatus(long jobExecutionQueueId, JobExecutionStatus status)
        {
            try
            {
                HttpResponseMessage responseMessage;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetAccessToken());
                    responseMessage = await client.PostAsync(new Uri(m_BaseUri, "api/JobExecutionQueue/UpdateJobExecutionStatus?jobExecutionQueueId=" + jobExecutionQueueId.ToString() + "&jobExecutionStatus=" + status.ToString()), null);
                    responseMessage.EnsureSuccessStatusCode();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CreatePowerShellJobResult(PowerShellJobResult powerShellJobResult)
        {
            try
            {
                HttpResponseMessage responseMessage;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetAccessToken());
                    responseMessage = await client.PostAsync(new Uri(m_BaseUri, "api/PowerShell/CreatePowerShellJobResult/"), powerShellJobResult, new JsonMediaTypeFormatter());
                    responseMessage.EnsureSuccessStatusCode();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateJobExecutionResult(long jobExecutionQueueId, JobExecutionStatus jobExecutionStatus, string resultMessage, DateTime dateCompleted, long totalExecutionTimeInMs)
        {
            HttpResponseMessage responseMessage;

            JobExecutionQueueResult jobResult = new JobExecutionQueueResult
            {
                DateCompleted = dateCompleted,
                Exception = null,
                JobExecutionQueueId = jobExecutionQueueId,
                Message = resultMessage,
                Result = ConvertJobExecutionStatusToResult(jobExecutionStatus),
                TotalExecutionTimeInMs = totalExecutionTimeInMs
            };

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetAccessToken());
                responseMessage = await client.PostAsync<JobExecutionQueueResult>(new Uri(m_BaseUri, "api/JobExecutionQueue/UpdateJobExecutionResult/"), jobResult, new JsonMediaTypeFormatter());
                responseMessage.EnsureSuccessStatusCode();
            }
        }

        private Result ConvertJobExecutionStatusToResult(JobExecutionStatus jobExecutionStatus)
        {
            switch (jobExecutionStatus)
            {
                case JobExecutionStatus.DOWNLOADED_BY_CLIENT:
                    break;
                case JobExecutionStatus.ERROR: return Result.ERROR;
                    break;
                case JobExecutionStatus.EXECUTING:
                    break;
                case JobExecutionStatus.FAILED:
                    break;
                case JobExecutionStatus.FATAL: return Result.FATAL;
                    break;
                case JobExecutionStatus.NOT_RECIEVED_BY_CLIENT:
                    break;
                case JobExecutionStatus.SUCCESS: return Result.SUCCESS;
                    break;
                case JobExecutionStatus.WARNING: return Result.WARNING;
                    break;
                default:
                    break;
            }
            return Result.ERROR;
        }

        public async Task AddJobExecutionLogEntry(long jobExecutionQueueId, DateTime date, LogLevel logLevel, string logger, string message, Exception exception = null)
        {
            try
            {
                JobExecutionLog logEntry = new JobExecutionLog
                    {
                        JobExecutionQueueId = jobExecutionQueueId,
                        Date = date,
                        Exception = exception != null ? exception.Message + " --- STACKTRACE: " + exception.StackTrace : string.Empty,
                        Logger = logger,
                        LogLevel = logLevel,
                        Message = message
                    };
                HttpResponseMessage responseMessage;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetAccessToken());
                    responseMessage = await client.PostAsync<JobExecutionLog>(new Uri(m_BaseUri, "api/Log/AddJobExecutionLogEntry/"), logEntry, new JsonMediaTypeFormatter());
                    responseMessage.EnsureSuccessStatusCode();
                }
            }
            catch (Exception e)
            {                
                throw;
            }
        }

        public async Task<IEnumerable<JobExecutionQueue>> GetJobsWaitingToExecute(Guid id, string ipAddress, string hostName)
        {
            IEnumerable<JobExecutionQueue> results;
            HttpResponseMessage responseMessage;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetAccessToken());

                responseMessage = await client.GetAsync(new Uri(m_BaseUri, "api/JobExecutionQueue/GetJobsWaitingToExecute?id=" + id.ToString() + "&ipAddress=" + ipAddress + "&hostName=" + hostName));
                responseMessage.EnsureSuccessStatusCode();
                results = await responseMessage.Content.ReadAsAsync<IEnumerable<JobExecutionQueue>>();
            }
            return results;
        }


        #region Private Methods

        private string GetAccessToken()
        {
            string result = GetCachedItem<string>("JOBENGINE_API_ACCESS_TOKEN");

            if (result == null) // get new access token
            {
                var pairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>( "grant_type", "password"), 
                    new KeyValuePair<string, string>( "username", m_Username), 
                    new KeyValuePair<string, string> ( "password", m_Password),
                    new KeyValuePair<string, string> ("jobengine_clientid", Settings.JobEngineClientId.ToString())
                };
                var content = new FormUrlEncodedContent(pairs);

                using (var client = new HttpClient())
                {
                    var tokenEndpoint = new Uri(m_BaseUri, "Token");

                    using (HttpResponseMessage response = client.PostAsync(tokenEndpoint, content).Result)
                    {
                        var responseContent = response.Content.ReadAsStringAsync().Result;
                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception(string.Format("Error: {0}", responseContent));
                        }
                        Dictionary<string, string> tokenDictionary = GetTokenDictionary(responseContent);
                        result = tokenDictionary["access_token"];

                        int secondsBeforeExpiration = 0;
                        if (int.TryParse(tokenDictionary["expires_in"], out secondsBeforeExpiration))
                        {
                            AddCacheItem("JOBENGINE_API_ACCESS_TOKEN", (secondsBeforeExpiration - 120), result);
                        }
                    }
                }
            }
            return result;
        }


        private Dictionary<string, string> GetTokenDictionary(string responseContent)
        {
            Dictionary<string, string> tokenDictionary =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(
                responseContent);
            return tokenDictionary;
        }

        private Type GetCachedItem<Type>(string key) where Type : class
        {
            ObjectCache cache = MemoryCache.Default;
            Type cachedValue = cache[key] as Type;

            if (cachedValue != null)
                return cachedValue;
            else
                return null;
        }

        private void AddCacheItem(string key, int expirationSeconds, object item)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now.AddSeconds(expirationSeconds);
            MemoryCache.Default.Add(key, item, policy);
        }

        private void RemoveCacheItem(string key)
        {
            MemoryCache.Default.Remove(key);
        }

        #endregion
    }
}
