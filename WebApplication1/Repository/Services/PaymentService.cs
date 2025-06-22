using Microsoft.EntityFrameworkCore;
using WebApplication1.Repository.Data;
using WebApplication1.Repository.Models;
using WebApplication1.Repository.Services.Contracts;

using ILogger= Serilog.ILogger;

namespace WebApplication1.Repository.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;
        private ILogger _logger;

        public PaymentService(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            return await _context.Payments.FindAsync(id);
        }

        public async Task<Payment> CreateAsync(Payment payment)
        {
            try
            {
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
                return payment;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"An error occurred while creating the payment: {payment.Amount} {payment.Date} for transaction {payment.TransactionId}");
                throw new Exception("An error occurred while creating the payment.", ex);
            }
        }

        public async Task<bool> UpdateAsync(Payment payment)
        {
            var entry = await _context.Payments.FindAsync(payment.PaymentId);
            if (entry == null)
            {
                _logger.Warning($"Payment with ID {payment.PaymentId} could not be found.");
                return false;
            }
            try
            {
                _context.Payments.Update(payment);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"An error occurred while updating the payment: {payment.PaymentId} for transaction {payment.TransactionId}");
                throw new Exception("An error occurred while updating the payment.", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entry = await _context.Payments.FindAsync(id);
            if (entry == null)
            {
                _logger.Warning($"Payment with ID {id} could not be found.");
                return false;
            }

            try
            {
                _context.Payments.Remove(entry);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"An error occurred while deleting the payment with ID: {id}");
                throw new Exception("An error occurred while deleting the payment.", ex);
            }

        }
    }
}
