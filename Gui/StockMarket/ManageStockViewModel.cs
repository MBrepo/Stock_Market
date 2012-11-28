using System.ComponentModel.Composition;
using System.Linq;
using App.StockMarket.Domain;
using App.StockMarket.Events.Message;
using Caliburn.Micro;
using Microsoft.Win32;

namespace App.StockMarket.Gui.StockMarket
{
    [Export(typeof(ManageStockViewModel))]
    public class ManageStockViewModel : Screen
    {
        #region Fields

        readonly IEventAggregator _events;

        #endregion

        #region Properties

        private Stock _stock;
        public Stock Stock
        {
            get { return _stock; }
            set
            {
                _stock = value;
                NotifyOfPropertyChange(() => Stock);
            }
        }
        #endregion

        #region Constructors

        [ImportingConstructor]
        public ManageStockViewModel(IEventAggregator events)
        {
            this._events = events;
        }

        #endregion

        #region Actions

        public void Save()
        {
            this.Stock.CurrentPrice = Stock.DefaultPrice;

            using(var context = new StockMarketContext())
            {
                var stock = context.AvailableStocks.Find(Stock.StockId);

                if(stock == null)
                    context.AvailableStocks.Add(Stock);
                else
                {
                    var toUpdate = context.AvailableStocks.Single(s => s.StockId == Stock.StockId);
                    toUpdate.Name = Stock.Name;
                }

                context.SaveChanges();
                Publish(Stock);

                base.TryClose(true);
            }
        }

        public void Delete()
        {
            using(var context = new StockMarketContext())
            {
                var stock = context.AvailableStocks.Single(s => s.StockId == Stock.StockId);
                
                stock.PriceHistory.Clear();
                context.AvailableStocks.Remove(stock);
                context.SaveChanges();

                Publish(Stock, true);

                base.TryClose(true);
            }
        }

        public void Cancel()
        {
            base.TryClose(false);
        }

        public void SelectImage()
        {
            var dialog = new OpenFileDialog {Filter = "Image Files|*.jpg;*.jpeg;*.png;*.png;*.gif"};
            var result = dialog.ShowDialog();
            if (result == true)
            {
                var fileName = dialog.FileName;
                Stock.ImageSrc = fileName;
            }
        }

        public void Publish(Stock stock, bool delete = false)
        {
            _events.Publish(new StockChangedEvent
            {
                Stock = stock,
                Delete = delete
            });
        }

        #endregion
    }
}
