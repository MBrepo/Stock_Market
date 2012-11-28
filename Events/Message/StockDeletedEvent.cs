using App.StockMarket.Domain;

namespace App.StockMarket.Events.Message
{
    public class StockDeletedEvent
    {
        public Stock Stock { get; set; }
    }
}
