using System.ComponentModel.DataAnnotations;

public class ArticleUpdateDTO : ArticleCreateDTO
{
    [Required]
    public int ArticleId { get; set; }

}