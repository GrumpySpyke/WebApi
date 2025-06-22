using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Repository.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required,StringLength(100)]
        public string FirstName { get; set; } = null!;

        [Required, StringLength(100)]
        public string LastName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required, Phone]
        public string Phone { get; set; } = null!;
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    }
}
