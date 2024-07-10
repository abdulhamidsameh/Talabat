using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Infrastructure
{
	internal static class SpecificationsEvaulator<T> where T : BaseEntity
	{
		public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecifications<T> spec)
		{
			var query = inputQuery;
			if (spec.Criteria is not null)
				query = query.Where(spec.Criteria);

			spec.Count = query.Count();
			

			//query = spec.Includes.Aggregate(query,(currentQuery,includeExpression) => currentQuery.Include(includeExpression));

			if (spec.OrderBy is not null)
				query = query.OrderBy(spec.OrderBy);

			else if (spec.OrderByDesc is not null)
				query = query.OrderByDescending(spec.OrderByDesc);

				query = query.Skip(spec.Skip).Take(spec.Take);

			if (spec.Includes.Count > 0)
				foreach (var include in spec.Includes)
					query = query.Include(include);

			return query;
		}

	}
}
