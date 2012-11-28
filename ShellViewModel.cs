using App.StockMarket;
using App.StockMarket.Domain;
using App.StockMarket.Gui.Orders;
using App.StockMarket.Gui.StockMarket;
using Caliburn.Micro;

namespace App.StockMarket {
    using System.ComponentModel.Composition;

    [Export(typeof(IShell))]
    public class ShellViewModel : Screen, IShell
    {
        private IWindowManager WindowManager { get; set; }

        [Import]
        private StockMarketViewModel _stockMarket;
        public StockMarketViewModel StockMarket
        {
            get { return _stockMarket; }
            set
            {
                _stockMarket = value;
                NotifyOfPropertyChange(() => StockMarket);
            }
        }

        [ImportingConstructor]
        public ShellViewModel(IEventAggregator events)
        {
            this.WindowManager = IoC.Get<IWindowManager>();
            events.Subscribe(this);
        }

        public void AddProduct()
        {
            var manageStockViewModel = IoC.Get<ManageStockViewModel>();

            manageStockViewModel.Stock = new Stock();

            WindowManager.ShowDialog(manageStockViewModel);
        }

    }
}
