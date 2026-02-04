using BankRUs.Api.UseCases.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankRUs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "CustomerService")]
    public class CustomersController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCustomers(
        [FromServices] GetCustomersHandler handler,
        int page = 1,
        int pageSize = 20)
        {
            var result = await handler.Handle(page, pageSize);
            return Ok(result);
        }

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
    }
}
