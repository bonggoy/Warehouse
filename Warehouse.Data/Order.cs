using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Warehouse.Data.Attributes;

namespace Warehouse.Data
{
	[Table("Orders")]
	public class Order : IEntity
	{
		[Key]
		public int Id { get; set; }

		// ordered articles, the way that the articles are to be delivered
		[SqlDefaultValue(DefaultValue = "GetUtcDate()")]
		public DateTime DateOfCreation { get; set; }

		public int CustomerId { get; set; }
		[ForeignKey("CustomerId")]
		public virtual Customer Customer { get; set; }

		public virtual Collection<Article> Articles { get; set; }

		public int DeliveryId { get; set; }
		[ForeignKey("DeliveryId")]
		public virtual Delivery Delivery { get; set; }
	}
}
