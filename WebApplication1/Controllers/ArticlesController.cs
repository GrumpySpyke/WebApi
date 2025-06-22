using Microsoft.AspNetCore.Mvc;
using WebApplication1.Repository.Models;
using WebApplication1.Repository.Services.Contracts;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/articles")]
    public class ArticlesController: ControllerBase
    {
        private readonly IArticleService _service;
        public ArticlesController(IArticleService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var article = await _service.GetByIdAsync(id);
            return article is null
                ? NotFound(new { error = $"Article {id} not found" })
                : Ok(article);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ArticleCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var article = new Article
            {
                Name = dto.Name,
                Stock = dto.Stock,
                Price = dto.Price
            };

            var created = await _service.CreateAsync(article);
            return CreatedAtAction(nameof(GetById), new { id = created.ArticleId }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Article article)
        {
            if (id != article.ArticleId)
                return BadRequest(new { error = "ID in path must match body request" });

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var updated = await _service.UpdateAsync(article);
            return updated
                ? NoContent()
                : NotFound(new { error = $"Article {id} not found" });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted
                ? NoContent()
                : NotFound(new { error = $"Article {id} not found" });
        }
    }
}
