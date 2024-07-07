using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Infrastructure.Data;

namespace Talabat.Infrastructure
{
	public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{
		private readonly StoreContext _dbContext;

		public GenericRepository(StoreContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<T>> GetAllAsync()
			=>  await _dbContext.Set<T>().AsNoTracking().ToListAsync();

		public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
			=>  await ApplySpecifications(spec).AsNoTracking().ToListAsync();
		
		public async Task<T?> GetAsync(int id)
			=> await _dbContext.Set<T>().FindAsync(id);

		public async Task<T?> GetWithSpecAsync(ISpecifications<T> spec)
			=>  await ApplySpecifications(spec).FirstAsync();

		private IQueryable<T> ApplySpecifications(ISpecifications<T> spec)
			=> SpecificationsEvaulator<T>.GetQuery(_dbContext.Set<T>(), spec);
	}
}
