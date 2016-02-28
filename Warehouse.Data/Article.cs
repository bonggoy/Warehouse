using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Data
{
	[Table("Articles")]
	public class Article : IEntity
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(250)]
		public string Code { get; set; }

		[Required]
		[StringLength(250)]
		public string Name { get; set; }

		[Required]
		public decimal Price { get; set; }

		[Required]
		public int ExpiryDays { get; set; }

		public virtual Collection<Device> Devices { get; set; }

		[NotMapped]
		public int DevicesCount { get; set; }
	}
}
