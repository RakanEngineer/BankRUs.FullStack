using BankRUs.Api.UseCases.Customers;
using BankRUs.Application.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace BankRUs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly GetCustomersHandler _handler;
        public CustomersController(GetCustomersHandler handler)
        {
            _handler = handler;
        }

        // GET /api/customers?page=1&pageSize=20
        //[HttpGet]
        //public async Task<IActionResult> GetCustomers(
        //[FromServices] GetCustomersHandler handler,
        //int page = 1,
        //int pageSize = 20,
        //[FromQuery] string? ssn = null)
        //{
        //    var result = await handler.Handle(page, pageSize);
        //    return Ok(result);
        //}

        [Authorize(Roles = Roles.CustomerService)]
        // GET /api/customers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(
            string id,
            [FromServices] GetCustomerByIdHandler handler)
        {
            var result = await handler.Handle(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [Authorize(Roles = Roles.CustomerService)]
        // GET /api/customers?ssn=19900101&page=1&pageSize=20
        [HttpGet]
        public async Task<IActionResult> GetCustomers(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? ssn = null)
        {
            var query = new GetCustomerQuery(page, pageSize, ssn);

            var result = await _handler.Handle(query);

            return Ok(result); 
        }

        [Authorize(Roles = Roles.Customer)]
        [HttpPatch]
        public async Task<IActionResult> UpdateCustomer(
            [FromBody] UpdateAccountDetailsCommand command,
            [FromServices] UpdateAccountDetailsHandler handler)
        {
            var success = await handler.Handle(command);

            if (!success)
                return BadRequest();

            return NoContent();
        }
    }
}
