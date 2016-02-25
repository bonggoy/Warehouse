using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Data
{
	[Table("Devices")]
	public class Device : IEntity
	{
		[Key]
		public int Id { get; set; }

		public int ArticleId { get; set; }
		[ForeignKey("ArticleId")]
		public virtual Article Article { get; set; }

		[Required]
		[StringLength(250)]
		public string Code { get; set; }
	}
}
