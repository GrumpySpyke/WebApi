using System.ComponentModel.DataAnnotations;

public class CustomerUpdateDTO : CustomerCreateDTO
{
    [Required]
    public int CustomerId { get; set; }
}