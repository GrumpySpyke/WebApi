namespace WebApplication1.Repository.Models
{
    public class TransactionArticle
    {
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; } = null!;
        public int ArticleId { get; set; }
        public Article Article { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
