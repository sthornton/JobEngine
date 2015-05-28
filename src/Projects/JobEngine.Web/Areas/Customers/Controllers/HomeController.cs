using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobEngine.Models;
using JobEngine.Core.Persistence;
using AutoMapper;
using JobEngine.Web.Areas.Customers.Models;

namespace JobEngine.Web.Areas.Customers.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private static ICustomerRepository customerRepository = RepositoryFactory.GetCustomerRepository();

        public ActionResult Index()
        {
            var customers = customerRepository.GetAll();
            var viewModel = Mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerViewModel>>(customers);
            return View(viewModel.OrderBy(x => x.Name));
        }

        public ActionResult Details(Guid? id)
        {
            if (id.HasValue)
            {
                var customer = customerRepository.Get(id.Value);
                var viewModel = Mapper.Map<Customer, CustomerViewModel>(customer);
                return View(viewModel);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="CustomerId,Name,IsDeleted,DateModified,ModifiedBy,DateCreated")] CustomerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                customerRepository.Create(viewModel.Name, User.Identity.Name);
                SuccessMessage = "Customer created successfully";
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public ActionResult Edit(Guid? id)
        {
            if(id.HasValue)
            {
                var customer = customerRepository.Get(id.Value);
                var viewModel = Mapper.Map<Customer, CustomerViewModel>(customer);
                return View(viewModel);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="CustomerId,Name,IsDeleted,DateModified,ModifiedBy,DateCreated")] CustomerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var customer = customerRepository.Get(viewModel.CustomerId);
                customer.Name = viewModel.Name;
                customer.ModifiedBy = User.Identity.Name;
                customer.DateModified = DateTime.UtcNow;
                customerRepository.Edit(customer);
                SuccessMessage = "Customer modified successfully";
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id.HasValue)
            {
                var customer = customerRepository.Get(id.Value);
                var viewModel = Mapper.Map<Customer, CustomerViewModel>(customer);
                return View(viewModel);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            customerRepository.Delete(id);
            SuccessMessage = "Customer deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
