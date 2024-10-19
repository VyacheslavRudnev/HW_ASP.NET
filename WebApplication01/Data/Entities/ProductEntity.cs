using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplication01.Data.Entities;

namespace WebApplication01.Data.Entities;

[Table("tbl_products")] //Атрибут Table вказує на те, що цей клас буде відображатися в таблиці tbl_products в базі даних.
public class ProductEntity  //Клас ProductEntity представляє сутність продукту, яка зберігається в базі даних.
{
    [Key]
    public int Id { get; set; }
    [Required, StringLength(255)]
    public string Name { get; set; } = String.Empty;
    public decimal Price { get; set; }
    [ForeignKey("Category")] //Атрибут ForeignKey вказує на те, що властивість CategoryId є зовнішнім ключем,
                             //який посилається на властивість Id в класі CategoryEntity.
    public int CategoryId { get; set; }
    public CategoryEntity? Category { get; set; } //Навігаційна властивість, яке вказує на те, що продукт належить до певної категорії.
    public virtual ICollection<ProductImageEntity>? ProductImages { get; set; } //Навігаційна властивість, яка вказує на те, що продукт має декілька зображень.
}