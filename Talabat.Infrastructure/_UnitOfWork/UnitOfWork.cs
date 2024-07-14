using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.UnitOfWork.Contract;
using Talabat.Infrastructure.Data;
using Talabat.Infrastructure.Generic_Repository;

namespace Talabat.Infrastructure._UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly StoreContext _dbContext;

		private Hashtable _repositories;

		public UnitOfWork(StoreContext dbContext)
		{
			_dbContext = dbContext;
			_repositories = new Hashtable();
		}

		public async Task<int> CompleteAsync()
		{
			return await _dbContext.SaveChangesAsync();
		}

		public async ValueTask DisposeAsync()
		{
			await _dbContext.DisposeAsync();
		}

		public IGenericRepository<T> Repository<T>() where T : BaseEntity
		{
			var key = typeof(T).Name;

			if (!_repositories.ContainsKey(key))
			{
				var repo = new GenericRepository<T>(_dbContext);
				_repositories.Add(key, repo);
			}

			return _repositories[key] as IGenericRepository<T>;
		}
	}
}
