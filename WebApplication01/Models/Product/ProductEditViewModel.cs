using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApplication01.Models.Product;

public class ProductEditViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Назва продукту є обов'язковою.")]
    [Display(Name = "Назва продукту")]
    public string Name { get; set; } = string.Empty;
   
    [Required(ErrorMessage = "Оберіть категорію.")]
    [Display(Name = "Category")]
    public int CategoryId { get; set; }
    public SelectList? CategoryList { get; set; } //вбудований клас для відображення списку категорій
    public List<string> ExistingImages { get; set; } = new List<string>(); //список шляхів до існуючих фото
    public List<IFormFile> Photos { get; set; } = new List<IFormFile>();
    
    [Required(ErrorMessage = "Вкажіть ціну.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Ціна має бути більше нуля.")]
    [Display(Name = "Ціна")]
    public string Price { get; set; } = string.Empty;




}
