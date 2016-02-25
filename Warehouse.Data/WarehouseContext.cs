using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Warehouse.Data.Attributes;

namespace Warehouse.Data
{
	public class WarehouseContext : DbContext
	{
		public DbSet<Article> Articles { get; set; }
		public DbSet<Device> Devices { get; set; }
		public DbSet<Order> Orders { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions
				// SqlDefaultValueAttribute
				.Add(new AttributeToColumnAnnotationConvention<SqlDefaultValueAttribute, string>("SqlDefaultValue", (p, attributes) => attributes.Single().DefaultValue));

			//modelBuilder.Entity<Article>()
			//	.Property(e => e.Code)
			//	.IsFixedLength()
			//	.IsUnicode(false);
		}
	}
}
