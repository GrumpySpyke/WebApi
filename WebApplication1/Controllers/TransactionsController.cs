using Microsoft.AspNetCore.Mvc;
using WebApplication1.Common.DTO;
using WebApplication1.Repository.Services.Contracts;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    public class TransactionsController : ControllerBase
    {
        private ITransactionService _service;

        public TransactionsController(ITransactionService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            return dto is null
                ? NotFound(new { error = $"Transaction {id} not found" })
                : Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NewTransactionDTO dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTransactionDTO dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var updated = await _service.UpdateAsync(dto);

            return updated
                ? NoContent()
                : NotFound(new { error = $"Transaction {dto.TransactionId} not found" });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted
                ? NoContent()
                : NotFound(new { error = $"Transaction {id} not found" });
        }

    }
}
