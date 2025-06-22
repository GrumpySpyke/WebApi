using Microsoft.EntityFrameworkCore;
using WebApplication1.Repository.Data;
using WebApplication1.Repository.Models;
using WebApplication1.Repository.Services.Contracts;

using ILogger= Serilog.ILogger;

namespace WebApplication1.Repository.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;
        private ILogger _logger;

        public CustomerService(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            try
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return customer;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"An error occurred while creating the customer: {customer.FirstName} {customer.LastName}");
                throw new Exception("An error occurred while creating the customer.", ex);
            }
        }

        public async Task<bool> UpdateAsync(Customer customer)
        {
            var entry = await _context.Customers.FindAsync(customer.CustomerId);
            if (entry == null)
            {
                _logger.Warning($"Customer with ID {customer.CustomerId} could not be found.");
                return false;
            }
            try
            {
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"An error occurred while updating the customer: {customer.FirstName} {customer.LastName}");
                throw new Exception("An error occurred while updating the customer.", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entry = await _context.Customers.FindAsync(id);
            if (entry == null)
            {
                _logger.Warning($"Customer with ID {id} could not be found.");
                return false;
            }

            try
            {
                _context.Customers.Remove(entry);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"An error occurred while deleting the customer with ID: {id}");
                throw new Exception("An error occurred while deleting the customer.", ex);
            }

        }

    }
}
