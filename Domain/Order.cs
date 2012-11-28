using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.StockMarket.Domain
{
    public class Order
    {
        public List<StockOrder> StockOrders { get; set; }
    }
}
