using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Data
{
	[Table("Customers")]
	public class Customer : IEntity
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(250)]
		public string Name { get; set; }

		[Required]
		[StringLength(250)]
		public string Surname { get; set; }

		[Required]
		[StringLength(250)]
		public string Address { get; set; }
	}
}