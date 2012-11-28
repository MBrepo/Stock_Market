using System.Collections.Generic;
using System.ComponentModel;
using App.StockMarket.Infrastructure.Validators;

namespace App.StockMarket.Domain
{
    public class Stock : INotifyPropertyChanged
    {
        public int StockId { get; set; }
        public string Name { get; set; }
        private string _imageSrc;
        public string ImageSrc
        {
            get { return _imageSrc; }
            set
            {
                _imageSrc = value;
                OnPropertyChanged("ImageSrc");
            }
        }

        public decimal DefaultPrice { get; set; }
        public decimal MinimumPrice { get; set; }
        public decimal MaximumPrice { get; set; }

        public bool SoldOut { get; set; }
        public int NumberInStock { get; set; }

        public decimal Profit { get { return DefaultPrice - CurrentPrice; } }

        private decimal _currentPrice;
        public decimal CurrentPrice
        {
            get
            {
                if (_currentPrice == 0)
                    _currentPrice = DefaultPrice;

                return _currentPrice;
            }
            set
            {
                _currentPrice = value;
                OnPropertyChanged("CurrentPrice");
            } 
        }

        private ICollection<StockPriceHistory> _priceHistory;
        public virtual ICollection<StockPriceHistory> PriceHistory
        {
            get
            {
                if (_priceHistory == null)
                    _priceHistory = new List<StockPriceHistory>();

                return _priceHistory;
            }
            set
            {
                _priceHistory = value;
                OnPropertyChanged("PriceHistory");
            }
        }

        public Stock()
        {
            
        }

        public Stock(string name, decimal defaultPrice, decimal minimumPrice, decimal maximumPrice)
        {
            this.Name = name;
            this.DefaultPrice = defaultPrice;
            this.MinimumPrice = minimumPrice;
            this.MaximumPrice = maximumPrice;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
