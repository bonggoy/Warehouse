namespace Warehouse.Data
{
	public interface IUnitOfWork
	{
		// IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity, new();
		IRepository<Article> ArticlesRepository { get; }
		IRepository<Order> OrdersRepository { get; }
		IRepository<Device> DevicesRepository { get; }
		void SaveChanges();
	}
}
