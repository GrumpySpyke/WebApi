using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Common.DTO
{
    public partial class NewTransactionDTO
    {
        [Required]
        public int CustomerId { get; set; }

        [Required, MinLength(1)]
        public List<ArticleDTO> Articles { get; set; } = new();

        [Required, MinLength(1)]
        public List<PaymentDTO> Payments { get; set; } = new();

      
    }
}
