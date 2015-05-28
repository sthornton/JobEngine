using JobEngine.Core.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.IO.Compression;

namespace JobEngine.Core.WebApi.Controllers
{
    [Authorize]
    public class FilesHandlerController : ApiController
    {
        private IAssemblyJobRepository assemblyJobRepository = RepositoryFactory.GetAssemblyJobRepository();

        [HttpPost]
        public HttpResponseMessage SynchAssemblies([FromUri]Guid jobEngineClientId, Dictionary<string, DateTime> files)
        {
            try
            {
                string tempFolderName = Guid.NewGuid().ToString();
                string tempDir = Path.Combine(Settings.TempDirectory, Guid.NewGuid().ToString());
                var assemblyJobs = assemblyJobRepository.GetAll();

                bool isSendingFilesDownToClient = false;
                foreach (var assemblyJob in assemblyJobs)
                {
                    if (files.ContainsKey(assemblyJob.PluginFileName))
                    {
                        DateTime lastModifiedTimeOnClient = files[assemblyJob.PluginFileName];
                        if (lastModifiedTimeOnClient < assemblyJob.DateModified)
                        {
                            isSendingFilesDownToClient = true;
                            if (!Directory.Exists(tempDir))
                                Directory.CreateDirectory(tempDir);
                            string fullTempFilePath = Path.Combine(tempDir, assemblyJob.PluginFileName);
                            File.WriteAllBytes(fullTempFilePath, assemblyJob.PluginFile);
                        }
                    }
                    else
                    {
                        isSendingFilesDownToClient = true;
                        if (!Directory.Exists(tempDir))
                            Directory.CreateDirectory(tempDir);
                        string fullTempFilePath = Path.Combine(tempDir, assemblyJob.PluginFileName);
                        File.WriteAllBytes(fullTempFilePath, assemblyJob.PluginFile);
                    }
                }

                var response = new HttpResponseMessage();

                if (isSendingFilesDownToClient)
                {
                    string zipFileFullPath = Path.Combine(Settings.TempDirectory, tempFolderName + ".zip");
                    ZipFile.CreateFromDirectory(tempDir, zipFileFullPath);                 
                    FileStream fileStream = File.Open(zipFileFullPath, FileMode.Open, FileAccess.Read);
                    response.Content = new StreamContent(fileStream);
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = tempFolderName + ".zip";
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response.Content.Headers.ContentLength = new FileInfo(zipFileFullPath).Length;                  
                    return response;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            
        }
    }
}
