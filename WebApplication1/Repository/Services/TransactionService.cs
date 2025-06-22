using Microsoft.EntityFrameworkCore;
using System;
using WebApplication1.Common.DTO;
using WebApplication1.Repository.Data;
using WebApplication1.Repository.Models;
using WebApplication1.Repository.Services.Contracts;

using ILogger = Serilog.ILogger;

namespace WebApplication1.Repository.Services
{
    public class TransactionService : ITransactionService
    {

        private readonly ApplicationDbContext _context;
        private ILogger _logger;

        public TransactionService(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<IEnumerable<TransactionDTO>> GetAllAsync()
        {
            var list = await _context.Transactions
                .Include(t => t.TransactionArticles)
                .Include(t => t.Payments)
                .ToListAsync();
            return list.Select(t => new TransactionDTO(t));
        }

        public async Task<TransactionDTO?> GetByIdAsync(int id)
        {
            var t = await _context.Transactions
                .Include(tx => tx.TransactionArticles)
                .Include(tx => tx.Payments)
                .FirstOrDefaultAsync(tx => tx.TransactionId == id);
            return t is null ? null : new TransactionDTO(t);
        }

        public async Task<TransactionDTO> CreateAsync(NewTransactionDTO dto)
        {
            // validate customer
            var customer = await _context.Customers.FindAsync(dto.CustomerId);
            if(customer == null)
            {
                _logger.Error($"Customer {dto.CustomerId} not found.");
                throw new KeyNotFoundException($"Customer {dto.CustomerId} not found.");
            }

            var transaction = new Transaction { CustomerId = dto.CustomerId, Date = DateTime.UtcNow };
            _context.Transactions.Add(transaction);

            //articles
            foreach (var item in dto.Articles)
            {
                var article = await _context.Articles.FindAsync(item.ArticleId);

                if (article == null)
                {
                    _logger.Error($"Article {item.ArticleId} not found.");
                    throw new KeyNotFoundException($"Article {item.ArticleId} not found.");
                }

                if (article.Stock < item.Quantity)
                {
                    _logger.Error($"Not enough items for {item.ArticleId}. Requested: {item.Quantity}, Available: {article.Stock}");
                    throw new InvalidOperationException($"Insufficient stock for article {item.ArticleId}.");
                }

                article.Stock -= item.Quantity;
                transaction.TransactionArticles.Add(
                    new TransactionArticle { ArticleId = item.ArticleId, Quantity = item.Quantity });
            }

            //payments
            foreach (var p in dto.Payments)
            {
                transaction.Payments.Add(new Payment { Amount = p.Amount, Date = DateTime.UtcNow });
            }

            await _context.SaveChangesAsync();

            return new TransactionDTO(transaction);
        }

        public async Task<bool> UpdateAsync(UpdateTransactionDTO dto)
        {
            var existing = await _context.Transactions
                .Include(tx => tx.TransactionArticles)
                .Include(tx => tx.Payments)
                .FirstOrDefaultAsync(tx => tx.TransactionId == dto.TransactionId);

            if (existing == null)
            {
                _logger.Warning($"Transaction with ID {dto.TransactionId} could not be found.");
                return false;
            }
            // reset related collections
            _context.TransactionArticles.RemoveRange(existing.TransactionArticles);
            _context.Payments.RemoveRange(existing.Payments);

            // update simple fields
            existing.CustomerId = dto.CustomerId;
            existing.Date = DateTime.UtcNow;

            // readd articles
            foreach (var item in dto.Articles)
            {
                var article = await _context.Articles.FindAsync(item.ArticleId);

                if (article == null)
                {
                    _logger.Error($"Article {item.ArticleId} not found.");
                    throw new KeyNotFoundException($"Article {item.ArticleId} not found.");
                }

                if (article.Stock < item.Quantity)
                {
                    _logger.Error($"Not enough items for {item.ArticleId}. Requested: {item.Quantity}, Available: {article.Stock}");
                    throw new InvalidOperationException($"Insufficient stock for article {item.ArticleId}.");
                }
                article.Stock -= item.Quantity;
                existing.TransactionArticles.Add(new TransactionArticle
                {
                    ArticleId = item.ArticleId,
                    Quantity = item.Quantity
                });
            }

            // readd payments
            foreach (var p in dto.Payments)
            {
                existing.Payments.Add(new Payment { Amount = p.Amount, Date = DateTime.UtcNow });
            }

            _context.Transactions.Update(existing);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Transactions.FindAsync(id);
            if (existing == null)
            {
                _logger.Warning($"Transaction with ID {id} could not be found.");
                return false;
            }

            _context.Transactions.Remove(existing);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
