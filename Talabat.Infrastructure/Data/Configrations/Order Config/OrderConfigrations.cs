using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Infrastructure.Data.Configrations.Order_Config
{
	internal class OrderConfigrations : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.OwnsOne(O => O.ShippingAddress, A => A.WithOwner());

			builder.Property(O => O.Status)
				.HasConversion(
				OStatus => OStatus.ToString(),
				OStatus => (OrderStatus) Enum.Parse(typeof(OrderStatus),OStatus)
				);

			builder.Property(O => O.SubTotal)
				.HasColumnType("decimal(12,2)");

		}
	}
}
