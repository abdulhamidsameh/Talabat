using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
	public class Order : BaseEntity
	{
		public Order(string buyerEmail, Address shippingAddress, decimal subTotal, ICollection<OrderItem> items, DeliveryMethod? deliveryMethod)
		{
			BuyerEmail = buyerEmail;
			ShippingAddress = shippingAddress;
			SubTotal = subTotal;
			Items = items;
			DeliveryMethod = deliveryMethod;
		}

		public Order()
        {
            
        }
        public string BuyerEmail { get; set; } = null!;
		public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;

		public OrderStatus Status { get; set; } = OrderStatus.Pending;

		public Address ShippingAddress { get; set; } = null!;

		public decimal SubTotal { get; set; }

		public string PaymentIntentId { get; set; } = string.Empty;

		public virtual ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>(); //NP

		public virtual int DeliveryMethodId { get; set; } //FK
		public virtual DeliveryMethod? DeliveryMethod { get; set; } = null!; //NP

		public decimal GetTotal() => SubTotal + DeliveryMethod!.Cost;
	}
}
