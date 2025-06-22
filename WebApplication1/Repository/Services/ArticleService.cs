using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApplication1.Repository.Data;
using WebApplication1.Repository.Models;
using WebApplication1.Repository.Services.Contracts;

using ILogger = Serilog.ILogger;

namespace WebApplication1.Repository.Services
{
    public class ArticleService : IArticleService
    {

        private readonly ApplicationDbContext _context;
        private ILogger _logger;

        public ArticleService(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<IEnumerable<Article>> GetAllAsync()
        {

            return await _context.Articles.ToListAsync();
        }

        public async Task<Article?> GetByIdAsync(int id)
        {
            return await _context.Articles.FindAsync(id);
        }

        public async Task<Article> CreateAsync(Article article)
        {
            try
            {
                _context.Articles.Add(article);
                await _context.SaveChangesAsync();
                return article;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"An error occurred while creating the article: {article.Name}");
                throw new Exception("An error occurred while creating the article.", ex);
            }
        }

        public async Task<bool> UpdateAsync(Article article)
        {
            var entry = await _context.Articles.FindAsync(article.ArticleId);
            if (entry == null)
            {
                _logger.Warning($"Article with ID {article.ArticleId} could not be found.");
                return false;
            }
            try
            {
                _context.Articles.Update(article);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"An error occurred while updating the article: {article.Name}");
                throw new Exception("An error occurred while updating the article.", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entry = await _context.Articles.FindAsync(id);
            if (entry == null)
            {
                _logger.Warning($"Article with ID {id} could not be found.");
                return false;
            }

            try
            {
                _context.Articles.Remove(entry);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"An error occurred while deleting the article with ID: {id}");
                throw new Exception("An error occurred while deleting the article.", ex);
            }

        }

    }
}
