using BankRUs.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankRUs.Api.Controllers;

// GET | POST | PUT ... /api/customers
[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomersController(CustomerService customerService)
        => _customerService = customerService;

    // GET /api/customers
    [HttpGet]
    public ActionResult Get()
    {
        // 200 OK -> JSON
        return Ok(_customerService.GetCustomers());
    }

    // GET /api/customers/{id}
    [HttpGet("{id}")]
    public ActionResult GetById(int id)
    {
        var customer = _customerService.FindCustomerById(id);

        if (customer is null)
        {
            // 404 Not Found
            return NotFound();
        }

        // 200 OK -> JSON (XML)
        return Ok(customer);
    }

    // POST /api/customers
    public ActionResult Create([FromBody] Customer customer)
    {
        var newCustomer = _customerService.CreateCustomer(customer.FirstName, customer.LastName);

        // 201 Created 
        return CreatedAtAction(nameof(GetById), new { newCustomer.Id }, newCustomer);
    }

    // DELETE /api/customers/1
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var customer = _customerService.DeleteCustomer(id);

        if (!customer)
        {
            // 404 Not Found
            return NotFound();
        }

        // 204 No Content
        return NoContent();
    }
}
