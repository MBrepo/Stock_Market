using App.StockMarket.Domain;

namespace App.StockMarket.Events.Message
{
    public class StockOrderPlacedEvent
    {
        public StockOrder StockOrder { get; set; }
        public bool Added { get; set; }

        public StockOrderPlacedEvent(StockOrder stockOrder, bool added)
        {
            this.StockOrder = stockOrder;
            this.Added = added;
        }
    }
}
