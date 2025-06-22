using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Common.DTO
{
    public class ArticleDTO
    {

        public ArticleDTO(int articleId, int quantity)
        {
            ArticleId = articleId;
            Quantity = quantity;
        }

        [Required]
        public int ArticleId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
