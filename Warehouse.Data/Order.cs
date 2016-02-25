using System;
using System.Collections.ObjectModel;

namespace Warehouse.Data
{
	public class Order : IEntity
	{
		public int Id { get; set; }

		// ordered articles, the way that the articles are to be delivered
		public DateTime DateOfCreation { get; set; }

		public int CustomerId { get; set; }
		public virtual Customer Customer { get; set; }

		public virtual Collection<Article> Articles { get; set; }

		public int DeliveryId { get; set; }
		public virtual Delivery Delivery { get; set; }
	}
}
