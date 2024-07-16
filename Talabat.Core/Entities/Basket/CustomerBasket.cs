﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Basket
{
    public class CustomerBasket
    {
        public CustomerBasket(string id)
        {
            Id = id;
            Items = new List<BasketItem>();
        }

        public string Id { get; set; } = null!;
        public List<BasketItem> Items { get; set; } = null!;
        public string? PaymentIntentId { get; set; } = null!;
        public string? ClientSecret { get; set; } = null!;

        public int? DeliveryMethodId { get; set; }

        public decimal ShippingPrice { get; set; }
    }
}
