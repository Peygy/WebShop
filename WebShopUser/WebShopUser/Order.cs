using System;
using System.Collections.Generic;
using System.Text;

namespace WebShopUser
{
    class Order
    {
        public int Id { get; set; }
        public Customer User { get; set; }
        public Basket UserBasket { get; set; }
    }
}
