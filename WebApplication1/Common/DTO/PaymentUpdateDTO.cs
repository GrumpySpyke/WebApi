using System.ComponentModel.DataAnnotations;

public class PaymentUpdateDTO : PaymentCreateDTO
{
    [Required]
    public int PaymentId { get; set; }

}