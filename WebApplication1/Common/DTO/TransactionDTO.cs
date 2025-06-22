using WebApplication1.Repository.Models;

namespace WebApplication1.Common.DTO
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public List<ArticleDTO> Articles { get; set; }
        public List<PaymentSummaryDTO> Payments { get; set; }

        public TransactionDTO(Transaction trans)
        {
            Id = trans.TransactionId;
            CustomerId = trans.CustomerId;
            Date = trans.Date;
            Articles = trans.TransactionArticles
                        .Select(ta => new ArticleDTO(ta.ArticleId, ta.Quantity))
                        .ToList();
            Payments = trans.Payments
                        .Select(p => new PaymentSummaryDTO(p.PaymentId,p.Amount, p.Date))
                        .ToList();
        }

        public TransactionDTO()
        {

        }
    }
}
