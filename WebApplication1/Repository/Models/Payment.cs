using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Repository.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; } = null!;
    }
}
