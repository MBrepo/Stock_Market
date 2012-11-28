using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using App.StockMarket.Domain;
using App.StockMarket.Events.Message;
using Caliburn.Micro;

namespace App.StockMarket.Gui.StockMarket
{
    [Export(typeof(StockMarketViewModel))]
    public class StockMarketViewModel : Screen, 
        IHandle<StockChangedEvent>,
        IHandle<OrderPlacedEvent>,
        IHandle<StockOrderPlacedEvent>
    {
        #region Fields

        private IWindowManager WindowManager { get; set; }
        readonly IEventAggregator _events;
        private BindableCollection<StockViewModel> _stocks;
        private Stock _selectedStock;

        #endregion

        #region Properties

        public BindableCollection<StockViewModel> Stocks
        {
            get { return _stocks; }
            set
            {
                _stocks = value;
                NotifyOfPropertyChange(() => Stocks);
            }
        }

        public Stock SelectedStock
        {
            get { return _selectedStock; }
            set
            {
                if (Equals(value, _selectedStock)) return;

                _selectedStock = value;
                NotifyOfPropertyChange(() => SelectedStock);
            }
        }

        public decimal TotalPrice
        {
            get { return StockOrders.Count > 0 ? StockOrders.Sum(s => s.Key.CurrentPrice * s.Value) : 0; }
        }

        public Dictionary<Stock, int> StockOrders { get; set; }

        #endregion

        #region Constructors

        [ImportingConstructor]
        public StockMarketViewModel(IEventAggregator events)
        {
            events.Subscribe(this);
            this.WindowManager = IoC.Get<IWindowManager>();
            this._events = events;
            Stocks = this.GetAvailableStock();

            StockOrders = new Dictionary<Stock, int>();
        }

        #endregion

        #region Actions

        public void FinalizeOrder()
        {
            using (var context = new StockMarketContext())
            {
                var order = new Order { StockOrders = Stocks.Select(s => s.Order.StockOrder).ToList() };
                context.PlaceOrder(order);

                _events.Publish(new OrderPlacedEvent
                {
                    Order = order
                });

                StockOrders.Clear();
                NotifyOfPropertyChange(() => TotalPrice);
            }
        }

        public void EditItem(StockViewModel stock)
        {
            var stockViewModel = IoC.Get<ManageStockViewModel>();

            stockViewModel.Stock = stock.Stock;
            WindowManager.ShowDialog(stockViewModel);
        }

        #endregion

        #region Implementation of IHandle events

        public void Handle(StockChangedEvent message)
        {
            var itemToHandle = Stocks.FirstOrDefault(s => s.Stock.StockId == message.Stock.StockId);
            if (itemToHandle == null)
                Stocks.Add(new StockViewModel(_events) { Stock = message.Stock});
            else
            {
                Stocks.Remove(itemToHandle);
                if(!message.Delete)
                    Stocks.Add(new StockViewModel(_events) {Stock = message.Stock});
            }
        }

        public void Handle(OrderPlacedEvent message)
        {
            Stocks = this.GetAvailableStock();
        }
        
        public void Handle(StockOrderPlacedEvent message)
        {
            int amount;
            if(StockOrders.TryGetValue(message.StockOrder.Stock, out amount))
            {
                StockOrders[message.StockOrder.Stock]  = message.StockOrder.Amount;
            }
            else
            {
                StockOrders.Add(message.StockOrder.Stock, message.StockOrder.Amount);
            }

            NotifyOfPropertyChange(() => TotalPrice);
        }

        #endregion

        #region Helper methods

        private BindableCollection<StockViewModel> GetAvailableStock()
        {
            using(var context = new StockMarketContext())
            {
                return new BindableCollection<StockViewModel>(
                    context.AvailableStocks.ToList().OrderBy(s => s.Name).Select(
                        s => new StockViewModel(_events)
                            {
                                Stock = s
                            }));
            }
        }

        #endregion
    }
}
