using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [ExceptionsFilter]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> GetCustomers()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(customers);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CustomerModel>> GetCustomerById(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound(new { message = "Customer not found" });
            }

            return Ok(customer);
        }

        [HttpGet("products/{id:int}")]
        public async Task<ActionResult<CustomerModel>> GetCustomerByProductId(int id)
        {
            var customer = await _customerService.GetCustomersByProductIdAsync(id);
            if (customer == null)
            {
                return NotFound(new { message = "Customer not found" });
            }

            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult> AddCustomer([FromBody] CustomerModel value)
        {
            await _customerService.AddAsync(value);
            return Ok(value);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateCustomer(int Id, [FromBody] CustomerModel value)
        {
            if (value == null || Id != value.Id)
            {
                return BadRequest();
            }

            await _customerService.UpdateAsync(value);
            return Ok(value);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            await _customerService.DeleteAsync(id);
            return Ok(Results.Json(customer));
        }
    }
}