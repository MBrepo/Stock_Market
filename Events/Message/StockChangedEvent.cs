using App.StockMarket.Domain;

namespace App.StockMarket.Events.Message
{
    public class StockChangedEvent
    {
        public Stock Stock { get; set; }
        public bool Delete { get; set; }
    }
}
