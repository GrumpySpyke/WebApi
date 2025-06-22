using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Common.DTO
{
    public class UpdateTransactionDTO : NewTransactionDTO
    {
        [Required]
        public int TransactionId { get; set; }

    }
}
