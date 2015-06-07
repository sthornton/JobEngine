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
        private IClientRepository clientRepository;
        private ICustomerRepository customerRepository;

        public JobEngineClientController(IClientRepository clientRepository, ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
            this.clientRepository = clientRepository;
        }

        public IEnumerable<JobEngineClient> GetAllClients()
        {
            return this.clientRepository.GetAll();
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return this.customerRepository.GetAll();
        }        
    }
}
