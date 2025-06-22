using WebApplication1.Repository.Models;

namespace WebApplication1.Repository.Services.Contracts
{
    public interface IArticleService
    {
        Task<IEnumerable<Article>> GetAllAsync();
        Task<Article?> GetByIdAsync(int id);
        Task<Article> CreateAsync(Article article);
        Task<bool> UpdateAsync(Article article);
        Task<bool> DeleteAsync(int id);
    }
}
