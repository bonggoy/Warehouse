using System;

namespace Warehouse.Data
{
	public class StoredArticle
	{
		public int ArticleId { get; set; }
		public Article Article { get; set; }
		public int Quantity { get; set; }
		public DateTime ExpiryDateOfQuarantee { get; set; }
	}
}
