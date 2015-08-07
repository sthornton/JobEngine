using JobEngine.Core.Persistence;
using JobEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace JobEngine.Core.WebApi.Controllers
{
    public class TestController : ApiController
    {
        private IClientRepository clientRepository;

        public TestController(IClientRepository clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public async Task<IEnumerable<string>> GetTest()
        {
            var results = await this.clientRepository.GetAllAsync();
            return results.Select(x => x.DateCreated.ToShortDateString()).ToList();
        }
    }
}
