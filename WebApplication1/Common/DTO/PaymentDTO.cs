using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Common.DTO
{
    public class PaymentDTO
    {
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

    }
}
