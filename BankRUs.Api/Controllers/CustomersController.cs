using BankRUs.Api.UseCases.Customers;
using BankRUs.Application.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using static BankRUs.Api.UseCases.Customers.DeleteCustomer;

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

        //[Authorize(Roles = Roles.CustomerService)]
        //// GET /api/customers?ssn=19900101&page=1&pageSize=20
        //[HttpGet]
        //public async Task<IActionResult> GetCustomers(
        //    [FromQuery] int page = 1,
        //    [FromQuery] int pageSize = 20,
        //    [FromQuery] string? ssn = null)
        //{
        //    var query = new GetCustomerQuery(page, pageSize, ssn);

        //    var result = await _handler.Handle(query);

        //    return Ok(result); 
        //}

        [HttpGet]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PagedResponseDto<CustomerItemDto>), StatusCodes.Status200OK)]
        [ProducesErrorResponseType(typeof(ProblemDetails))]
        [EndpointSummary("Get customers list")]
        [EndpointDescription("Returns paginated list of customers")]
        public async Task<IActionResult> GetCustomers(
            [FromQuery][Description("Page number")] int page = 1,
            [FromQuery][Description("Items per page")] int pageSize = 20,
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

        //[Authorize(Roles = Roles.Customer)]
        [HttpDelete("/api/me")]
        public async Task<IActionResult> DeleteMe([FromServices] DeleteCustomerHandler handler)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var success = await handler.Handle(
                new DeleteCustomerCommand(userId!));

            if (!success)
                return NotFound();

            return NoContent();
        }

        [Authorize(Roles = Roles.CustomerService)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(string id, [FromServices] DeleteCustomerHandler handler)
        {
            var success = await handler.Handle(
                new DeleteCustomerCommand(id));

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}