using JobEngine.Core.Persistence;
using JobEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace JobEngine.Core.Api.Areas.Api.Controllers
{
    [Authorize]
    public class JobEngineClientController : ApiController
    {       
        public IEnumerable<JobEngineClient> GetAllClients()
        {
            IClientRepository clientRepository = RepositoryFactory.GetClientRespository();
            return clientRepository.GetAll();
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            ICustomerRepository customerRepository = RepositoryFactory.GetCustomerRepository();
            return customerRepository.GetAll();
        }        
    }
}
