using System.ComponentModel.DataAnnotations;

public class ArticleCreateDTO
{
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, Range(0, int.MaxValue)]
    public int Stock { get; set; }

    [Required, Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }
}