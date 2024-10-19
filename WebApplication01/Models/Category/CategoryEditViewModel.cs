using System.ComponentModel.DataAnnotations;

namespace WebApplication01.Models.Category;

public class CategoryEditViewModel  //Модель подання для редагування категорії
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введіть назву категорії")]    //Атрибут Required вказує, що поле Name є обов'язковим для заповнення.
    public string Name { get; set; } = String.Empty;
    public string? Description { get; set; }
    public string? ExistingImage { get; set; }  //Назва файлу зображення, яке вже є у категорії
    public IFormFile? Photo { get; set; }   //Зображення, яке користувач може завантажити
}
    

    