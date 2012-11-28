using System.ComponentModel.Composition;
using System.Windows.Controls;
using App.StockMarket.Domain;
using App.StockMarket.Events.Message;
using Caliburn.Micro;

namespace App.StockMarket.Gui.Orders
{
    [Export(typeof(EmbeddedOrderViewModel))]
    public class EmbeddedOrderViewModel : Screen
    {
        #region Fields

        readonly IEventAggregator _events;

        #endregion
        
        #region Properties

        private StockOrder _stockOrder;
        public StockOrder StockOrder
        {
            get { return _stockOrder; }
            set
            {
                _stockOrder = value;
                NotifyOfPropertyChange(() => StockOrder);
            }
        }

        #endregion

        #region Constructors

        [ImportingConstructor]
        public EmbeddedOrderViewModel(IEventAggregator events, Stock stock)
        {
            this._events = events;
            events.Subscribe(this);

            StockOrder = new StockOrder {Stock = stock};
        }

        #endregion

        #region Actions

        public void UpdateOrder(TextBox textbox)
        {
            var value = textbox.Text;
            int amount;
            int.TryParse(value, out amount);

            StockOrder = new StockOrder
                {
                    Amount = amount,
                    Stock = StockOrder.Stock
                };

            _events.Publish(new StockOrderPlacedEvent(StockOrder, true));
        }

        public void SelectAll(TextBox textbox)
        {
            textbox.SelectAll();
        }

        public void IncreaseOrder()
        {
            StockOrder = new StockOrder
                             {
                                 Amount = ++StockOrder.Amount,
                                 Stock = StockOrder.Stock
                             };

            _events.Publish(new StockOrderPlacedEvent(StockOrder, true));
        }

        public void DecreaseOrder()
        {
            StockOrder = new StockOrder
                             {
                                 Amount = --StockOrder.Amount,
                                 Stock = StockOrder.Stock
                             };

            _events.Publish(new StockOrderPlacedEvent(StockOrder, false));
        }

        #endregion
    }
}
