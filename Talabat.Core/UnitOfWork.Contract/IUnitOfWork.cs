using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;

namespace Talabat.Core.UnitOfWork.Contract
{
	public interface IUnitOfWork : IAsyncDisposable
	{
		public IGenericRepository<T> Repository<T>() where T : BaseEntity;

		public Task<int> CompleteAsync(); 

	}
}
