using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace App.StockMarket.Domain
{
    public class StockOrder
    {
        public Stock Stock { get; set; }
        public int Amount { get; set; }
    }
}
