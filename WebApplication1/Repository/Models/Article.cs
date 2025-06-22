using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Repository.Models
{
    public class Article
    {
        [Key]
        public int ArticleId { get; set; }
        [Required,StringLength(100)]
        public string Name { get; set; }

        [Required,Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative integer.")]
        public int Stock { get; set; }

        [Required, Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        public ICollection<TransactionArticle> TransactionArticles { get; set; } = new List<TransactionArticle>();
    }
}
