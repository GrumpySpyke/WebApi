using WebApplication1.Common.DTO;

namespace WebApplication1.Repository.Services.Contracts
{
    public interface ITransactionService
    {

        Task<TransactionDTO> CreateAsync(NewTransactionDTO dto);
        Task<TransactionDTO?> GetByIdAsync(int id);
        Task<IEnumerable<TransactionDTO>> GetAllAsync();
        Task<bool> UpdateAsync(UpdateTransactionDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
