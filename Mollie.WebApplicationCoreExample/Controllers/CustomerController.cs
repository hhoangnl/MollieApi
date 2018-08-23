﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Models.Customer;
using Mollie.WebApplicationCoreExample.Models;
using Mollie.WebApplicationCoreExample.Services.Customer;
using Mollie.WebApplicationCoreExample.Services.Overview;

namespace Mollie.WebApplicationCoreExample.Controllers {
    public class CustomerController : Controller {
        private readonly IOverviewClient<CustomerResponse> _customerOverviewClient;
        private readonly ICustomerStorageClient _customerStorageClient;

        public CustomerController(IOverviewClient<CustomerResponse> customerOverviewClient, ICustomerStorageClient customerStorageClient) {
            this._customerOverviewClient = customerOverviewClient;
            this._customerStorageClient = customerStorageClient;
        }

        [HttpGet]
        public async Task<ViewResult> Index() {
            OverviewModel<CustomerResponse> model = await this._customerOverviewClient.GetList();
            return this.View(model);
        }

        [HttpGet]
        public async Task<ViewResult> ApiUrl([FromQuery]string url) {
            OverviewModel<CustomerResponse> model = await this._customerOverviewClient.GetList(url);
            return this.View(nameof(this.Index), model);
        }

        [HttpGet]
        public ViewResult Create() {
            CreateCustomerModel model = new CreateCustomerModel();
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerModel model) {
            if (!this.ModelState.IsValid) {
                return this.View();
            }

            await this._customerStorageClient.Create(model);
            return this.RedirectToAction(nameof(this.Index));
        }
    }
}