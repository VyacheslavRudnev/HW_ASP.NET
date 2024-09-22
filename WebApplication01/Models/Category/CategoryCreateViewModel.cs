using System.ComponentModel.DataAnnotations;

namespace WebApplication01.Models.Category;

public class CategoryCreateViewModel
{
    [Display(Name = "Назва категорії")]
    public string Name { get; set; } = String.Empty;
    //тип для передачі файлів на сервер- зі сторінки хочу отримати файл із <input type="file"/>
    [Display(Name = "Оберыть фото на ПК")]
    public IFormFile? Photo { get; set; }
    [Display(Name = "Короткий опис")]
    public string Description { get; set; } = string.Empty;
}
