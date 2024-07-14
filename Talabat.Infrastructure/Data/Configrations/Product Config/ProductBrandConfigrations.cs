using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Product;

namespace Talabat.Infrastructure.Data.Configrations.Product_Config
{
    internal class ProductBrandConfigrations : IEntityTypeConfiguration<ProductBrand>
	{
		public void Configure(EntityTypeBuilder<ProductBrand> builder)
		{
			builder.Property(PB => PB.Name)
				.IsRequired();
		}
	}
}
