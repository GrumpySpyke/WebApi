using Microsoft.AspNetCore.Mvc;
using WebApplication1.Common.DTO;
using WebApplication1.Repository.Models;
using WebApplication1.Repository.Services.Contracts;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _service;
        public PaymentsController(IPaymentService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var payment = await _service.GetByIdAsync(id);
            return payment is null
                ? NotFound(new { error = $"Payment {id} not found" })
                : Ok(payment);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var payment = new Payment
            {
                Amount = dto.Amount,
                TransactionId = dto.TransactionId,
                Date = DateTime.UtcNow
            };

            var created = await _service.CreateAsync(payment);
            return CreatedAtAction(nameof(GetById), new { id = created.PaymentId }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PaymentUpdateDTO dto)
        {
            if (id != dto.PaymentId)
                return BadRequest(new { error = "ID in path must match payload" });

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var payment = new Payment
            {
                PaymentId = dto.PaymentId,
                Amount = dto.Amount,
                TransactionId = dto.TransactionId,
                Date = DateTime.UtcNow
            };

            var updated = await _service.UpdateAsync(payment);
            return updated
                ? NoContent()
                : NotFound(new { error = $"Payment {id} not found" });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted
                ? NoContent()
                : NotFound(new { error = $"Payment {id} not found" });
        }
    }
}
