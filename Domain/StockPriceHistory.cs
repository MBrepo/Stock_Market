using System;


namespace App.StockMarket.Domain
{
    public class StockPriceHistory
    {
        public int StockPriceHistoryId { get; set; }
   
        public DateTime DateOfPlacement { get; set; }
        public decimal Price { get; set; }
    }
}
