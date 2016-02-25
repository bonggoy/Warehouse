using System;
using System.Collections.Generic;
using System.Linq;

namespace Warehouse.Data
{
	public class UnitOfWork : IUnitOfWork
	{
		private WarehouseContext _context = new WarehouseContext();
		private Dictionary<Type, IRepository> _repositories;

		public UnitOfWork()
		{
			_context = new WarehouseContext();
			_context.Configuration.LazyLoadingEnabled = true;
			_repositories = new Dictionary<Type, IRepository>();
		}

		#region IDisposable Support
		private bool disposedValue = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
					_context.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.
				_context = null;
				_repositories = null;

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		~UnitOfWork()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(false);
		}

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			GC.SuppressFinalize(this);
		}
		#endregion

		private IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity, new()
		{
			// Checks if the Dictionary Key contains the Model class
			if (_repositories.Keys.Contains(typeof(TEntity)))
			{
				// Return the repository for that Model class
				return _repositories[typeof(TEntity)] as IRepository<TEntity>;
			}

			// If the repository for that Model class doesn't exist, create it
			var repository = new GenericRepository<TEntity>(_context);

			// Add it to the dictionary
			_repositories.Add(typeof(TEntity), repository);

			return repository;
		}

		public IRepository<Article> ArticlesRepository
		{
			get
			{
				return GetRepository<Article>();
			}
		}

		public IRepository<Order> OrdersRepository
		{
			get
			{
				return GetRepository<Order>();
			}
		}

		public IRepository<Device> DevicesRepository
		{
			get
			{
				return GetRepository<Device>();
			}
		}

		public void SaveChanges()
		{
			_context.SaveChanges();
		}
	}
}
