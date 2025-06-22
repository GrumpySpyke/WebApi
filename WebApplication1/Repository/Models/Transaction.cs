using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Repository.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public ICollection<TransactionArticle> TransactionArticles { get; set; } = new List<TransactionArticle>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
