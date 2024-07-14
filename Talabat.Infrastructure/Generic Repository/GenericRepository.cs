using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Infrastructure.Data;

namespace Talabat.Infrastructure.Generic_Repository
{
	public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly StoreContext _dbContext;

		public GenericRepository(StoreContext dbContext)
		{
			_dbContext = dbContext;
		}

	

		public async Task<IReadOnlyList<T>> GetAllAsync()
			=>  await _dbContext.Set<T>().AsNoTracking().ToListAsync();

		public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
			=>  await ApplySpecifications(spec).AsNoTracking().ToListAsync();
		
		public async Task<T?> GetAsync(int id)
			=> await _dbContext.Set<T>().FindAsync(id);

		public async Task<T?> GetWithSpecAsync(ISpecifications<T> spec)
			=>  await ApplySpecifications(spec).FirstOrDefaultAsync();
		public void Add(T entity)
		{
			_dbContext.Set<T>().Add(entity);
		}

		public void Delete(T entity)
		{
			_dbContext.Set<T>().Remove(entity);
		}

		public void Update(T entity)
		{
			_dbContext.Set<T>().Update(entity);
		}

		private IQueryable<T> ApplySpecifications(ISpecifications<T> spec)
			=> SpecificationsEvaulator<T>.GetQuery(_dbContext.Set<T>(), spec);
	}
}
