using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Warehouse.Data
{
	public interface IRepository
	{
	}

	public interface IRepository<TEntity> : IRepository where TEntity : IEntity
	{
		void Attach(TEntity entity);
		void Add(TEntity entity);
		void AddRange(IEnumerable<TEntity> collection);
		TEntity FindById(int id);
		IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);
		IQueryable<TEntity> All();
		TEntity Delete(int id);
		TEntity Delete(TEntity entity);
		int Delete(Expression<Func<TEntity, bool>> expression);
		void Update(TEntity entity);
		IQueryable<TEntity> Include<TProperty>(Expression<Func<TEntity, TProperty>> path);

	}
}
