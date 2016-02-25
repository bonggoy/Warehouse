namespace Warehouse.Data
{
	public class Customer : IEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Address { get; set; }
	}
}