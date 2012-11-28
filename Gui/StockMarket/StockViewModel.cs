using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Media.Imaging;
using App.StockMarket.Domain;
using App.StockMarket.Gui.Orders;
using Caliburn.Micro;

namespace App.StockMarket.Gui.StockMarket
{
    [Export(typeof(StockViewModel))]
    public class StockViewModel : Screen
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
                Order = new EmbeddedOrderViewModel(_events, Stock);
                Series = new BindableCollection<KeyValuePair<int, decimal>>(
                    _stock.PriceHistory
                        .Skip(Math.Max(0, _stock.PriceHistory.Count() - 10))
                        .Take(_stock.PriceHistory.Count() < 10 ? _stock.PriceHistory.Count() : 10)
                        .Select((ph, index) => new KeyValuePair<int, decimal>(index, ph.Price)));

                StockImage = new BitmapImage(new Uri(_stock.ImageSrc ?? "pack://application:,,,/Images/beer_default.jpg"));
            }
        }

        private BitmapImage _stockImage;
        public BitmapImage StockImage 
        { 
            get 
            { 
                return _stockImage; 
            }
            set 
            { 
                _stockImage = value;
                NotifyOfPropertyChange(() => StockImage);
            }
        }

        private BindableCollection<KeyValuePair<int, decimal>> _series;
        public BindableCollection<KeyValuePair<int, decimal>> Series
        {
            get { return _series; }
            set
            {
                _series = value;
                NotifyOfPropertyChange(() => Series);
            }
        }

        private EmbeddedOrderViewModel _order;
        public EmbeddedOrderViewModel Order
        {
            get { return _order; }
            set { _order = value;
                NotifyOfPropertyChange(() => Order);
            }
        }

        #endregion

        #region constructors

        [ImportingConstructor]
        public StockViewModel(IEventAggregator events)
        {
            this._events = events;
        }

        #endregion
    }
}
