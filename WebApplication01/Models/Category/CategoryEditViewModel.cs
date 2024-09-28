using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WebApplication01.Models.Category;

public class CategoryEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введіть ім'я категорії")]
    public string Name { get; set; } = String.Empty;

    public string? Description { get; set; }

    public string? ExistingImage { get; set; }

    public IFormFile? Photo { get; set; }
}
