using System.ComponentModel.DataAnnotations;

namespace WebApplication01.Models.Category;

public class CategoryEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введіть назву категорії")]
    public string Name { get; set; } = String.Empty;
    public string? Description { get; set; }
    public string? ExistingImage { get; set; }
    public IFormFile? Photo { get; set; }
    public List<string> ImagePaths { get; set; } //Список шляхів до зображень
}
    

    