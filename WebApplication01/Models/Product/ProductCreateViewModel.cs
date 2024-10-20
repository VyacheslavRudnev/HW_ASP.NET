using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApplication01.Models.Product;

public class ProductCreateViewModel
{
    [Display(Name = "Назва продукту")]
    public string Name { get; set; } = string.Empty;
    //Тип для передачі файлі на сервер - із сторінки хочу отримати файл із <input type="file"/>
    [Display(Name = "Оберіть фото на ПК")]
    public List<IFormFile> Photos { get; set; } = new List<IFormFile>();
    [Display(Name = "Ціна")]
    public string Price { get; set; } = string.Empty;
    [Display(Name = "Category")]
    [Required(ErrorMessage = "Choose a category")]
    public int CategoryId { get; set; }
    public SelectList? CategoryList { get; set; } //вбудований клас для відображення списку
}
