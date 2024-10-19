using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication01.Data.Entities;

[Table("tbl_categories")] //Атрибут Table вказує на те, що цей клас буде відображатися в таблиці tbl_categories в базі даних.
public class CategoryEntity //Клас CategoryEntity представляє сутність категорії, яка зберігається в базі даних.
{
	[Key]//Атрибут Key вказує на те, що це поле є первинним ключем в таблиці.
    public int Id { get; set; }
	[Required, StringLength(255)]//Атрибут Required вказує на те, що це поле є обов'язковим для заповнення.
								 //Атрибут StringLength вказує на максимальну довжину рядка.
    public string Name { get; set; } = String.Empty; 
    [StringLength(500)]
	public string? Image { get; set; }
	[StringLength(4000)]
	public string? Description { get; set; }
}
