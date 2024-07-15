using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Product
{
	public class ProductServiceReturn
	{
		public IReadOnlyList<Product> Products { get; set; } = null!;
        public int Count { get; set; }
    }
}
