using WebApplication01.Data.Entities;

namespace WebApplication01.Models.Product;

public class ProductItemViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = String.Empty;
    public List<string>? Images { get; set; }
}
