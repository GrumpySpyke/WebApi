using Microsoft.AspNetCore.Mvc;
using WebApplication1.Common.DTO;
using WebApplication1.Repository.Models;
using WebApplication1.Repository.Services.Contracts;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;
        public CustomersController(ICustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _service.GetByIdAsync(id);
            return customer is null
                ? NotFound(new { error = $"Customer {id} not found" })
                : Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var customer = new Customer
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone
            };

            var created = await _service.CreateAsync(customer);
            return CreatedAtAction(nameof(GetById), new { id = created.CustomerId }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerUpdateDTO dto)
        {
            if (id != dto.CustomerId)
                return BadRequest(new { error = "ID in path must match body request" });

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var customer = new Customer
            {
                CustomerId = dto.CustomerId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone
            };

            var updated = await _service.UpdateAsync(customer);
            return updated
                ? NoContent()
                : NotFound(new { error = $"Customer {id} not found" });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted
                ? NoContent()
                : NotFound(new { error = $"Customer {id} not found" });
        }
    }
}
