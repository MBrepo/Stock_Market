using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using App.StockMarket.Domain;

namespace StockMarket
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<StockMarketContext>());
            Database.SetInitializer(new StockMarketDatabaseInitializer());
            InitializeComponent();
        }

        public class StockMarketDatabaseInitializer : DropCreateDatabaseIfModelChanges<StockMarketContext>
        {
            public StockMarketDatabaseInitializer()
            {
                
            }

            protected override void Seed(StockMarketContext context)
            {
                if (Debugger.IsAttached)
                {
                    context.AvailableStocks.Add(GenerateStock("stock 1", 1.5, 1, 2.5));
                    context.SaveChanges();
                }
            }

            private Stock GenerateStock(string name, double defaultPrice, double minimumPrice, double maximumuPrice)
            {
                var random = new Random();

                return new Stock(name, new decimal(defaultPrice), new decimal(minimumPrice), new decimal(maximumuPrice))
                    {
                        PriceHistory = new Collection<StockPriceHistory>(Enumerable.Range(1, 10).Select(
                            d => new StockPriceHistory
                                {
                                    DateOfPlacement = DateTime.Now.AddMinutes(d),
                                    Price = new decimal(Math.Round(random.NextDouble() * Math.Abs(maximumuPrice - minimumPrice) + minimumPrice, 2))

                                }).ToList())
                    };
            }

        }
    }
}
