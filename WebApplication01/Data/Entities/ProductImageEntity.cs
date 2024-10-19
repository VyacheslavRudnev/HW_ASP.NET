using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication01.Data.Entities;

[Table("tbl_product_images")] //Атрибут Table вказує на те, що цей клас буде відповідати таблиці tbl_product_images в базі даних.
public class ProductImageEntity //Клас ProductImageEntity представляє сутність зображення товару.
{
    [Key]
    public int Id { get; set; }
    [Required, StringLength(255)]
    public string Image { get; set; } = string.Empty;
    public int Priority { get; set; }
    [ForeignKey("Product")] //Атрибут ForeignKey вказує на те, що поле ProductId є зовнішнім ключем,
                            //який посилається на поле Id в таблиці tbl_products.
    public int ProductId { get; set; }
    public virtual ProductEntity? Product { get; set; } //Віртуальне поле Product вказує на те, що це поле є навігаційною властивістю,
                                                        //яка дозволяє звертатися до даних з іншої таблиці.
}
