using App.StockMarket.Domain;

namespace App.StockMarket.Events.Message
{
    public class OrderPlacedEvent
    {
        public Order Order { get; set; }
    }
}
