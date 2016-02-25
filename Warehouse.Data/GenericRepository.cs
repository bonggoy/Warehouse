using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Warehouse.Data
{
	public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity, new()
	{
		private DbContext _context;
		public GenericRepository(DbContext context)
		{
			_context = context;
		}

		private DbSet<TEntity> DbSet
		{
			get
			{
				return _context.Set<TEntity>();
			}
		}

		public void Attach(TEntity entity)
		{
			DbSet.Attach(entity);
		}

		public void Add(TEntity entity)
		{
			DbSet.Add(entity);
		}

		public void AddRange(IEnumerable<TEntity> collection)
		{
			DbSet.AddRange(collection);
		}

		public IQueryable<TEntity> All()
		{
			return DbSet.AsQueryable(); // Select(x => x);
		}

		public TEntity FindById(int id)
		{
			// return DbSet.Find(id);
			return DbSet.FirstOrDefault(x => x.Id == id);
		}

		public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
		{
			return DbSet.Where(expression).AsQueryable();
		}

		public TEntity Remove(int id)
		{
			var entity = new TEntity() { Id = id };
			DbSet.Attach(entity);
			return Remove(entity);

		}

		public TEntity Remove(TEntity entity)
		{
			return DbSet.Remove(entity);
		}

		public int Remove(Expression<Func<TEntity, bool>> expression)
		{
			return DbSet.Where(expression).Delete();
		}

		public void Update(TEntity entity)
		{
			_context.Entry<TEntity>(entity).State = EntityState.Modified;
		}
	}
}
