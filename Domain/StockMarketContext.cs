using System;
using System.Data.Entity;

namespace App.StockMarket.Domain
{
    public class StockMarketContext : DbContext
    {
        private const decimal DownPercentage = 0.10m;
        private const decimal UpPercentage = 0.2m;

        public DbSet<Stock> AvailableStocks { get; set; }

        public void PlaceStockOrder(StockOrder stockOrder)
        {
            var stock = AvailableStocks.Find(stockOrder.Stock.StockId);
            if (stock == null) return;
            if (stockOrder.Amount == 0) return;

            for (var i = 0; i < stockOrder.Amount; i++)
            {
                // Increase current price of ordered stocks.
                var toIncrease = (stock.MaximumPrice - stock.CurrentPrice) * UpPercentage;
                stock.CurrentPrice += toIncrease;
                

                // Decrease the prices of the other stocks.
                foreach (var availableStock in AvailableStocks)
                {
                    if (availableStock == stock) continue;

                    var toDecrease = (availableStock.CurrentPrice - availableStock.MinimumPrice) * DownPercentage;
                    availableStock.CurrentPrice -= toDecrease;
                }
            }

            // Adjust pricehistory of all stocks.
            foreach (var availableStock in AvailableStocks)
            {
                availableStock.PriceHistory.Add(new StockPriceHistory
                                                    {
                                                        DateOfPlacement = DateTime.Now,
                                                        Price = availableStock.CurrentPrice
                                                    });
            }

            this.SaveChanges();
        }

        public void PlaceOrder(Order order)
        {
            foreach (var stockOrder in order.StockOrders)
            {
                PlaceStockOrder(stockOrder);
            }
        }

        public void Refresh()
        {
            AvailableStocks.Load();
        }
    }
}
