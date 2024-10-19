using System.ComponentModel.DataAnnotations;

namespace WebApplication01.Models.Category;

public class CategoryCreateViewModel    //Модель подання для створення нової категорії
{
    [Display(Name = "Назва категорії")] //Атрибут Display вказує на те, як відображати це поле в представленні.
    public string Name { get; set; } = String.Empty;
    //тип для передачі файлів на сервер- зі сторінки хочу отримати файл із <input type="file"/>
    [Display(Name = "Оберіть фото")]
    public IFormFile? Photo { get; set; } //IFormFile - це інтерфейс, який представляє файл, завантажений на сервер.
    [Display(Name = "Короткий опис")]
    public string Description { get; set; } = string.Empty;
}
