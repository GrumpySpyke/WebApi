using System.ComponentModel.DataAnnotations;

public class PaymentCreateDTO
{
    [Required, Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    [Required]
    public int TransactionId { get; set; }
}