namespace Warehouse.Data
{
	public interface IUnitOfWork
	{
		// IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity, new();
		IRepository<Article> ArticlesRepository { get; }
		void SaveChanges();
	}
}
