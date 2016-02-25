using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Admin.Frontend.Models
{
	public class Article
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Code { get; set; }
		[Required]
		public decimal Price { get; set; }
		[Required]
		public int ExpiryDays { get; set; }
	}

	public class ArticlesList
	{
		public List<Article> Items { get; set; }
		public ArticleFilter Filter { get; set; }
	}

	public class Device
	{
		public int Id { get; set; }
		public int ArticleId { get; set; }
		public string Code { get; set; }
	}
}