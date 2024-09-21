using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication01.Data.Entities
{
	[Table("tbl_categories")]
	public class CategoryEntity
	{
		[Key]
		public int Id { get; set; }
		[Required, StringLength(255)]
		public string Name { get; set; } = String.Empty;
		[StringLength(500)]
		public string? Image { get; set; }
		[StringLength(4000)]
		public string? Description { get; set; }
	}
}
