using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MoneyMind
{
    public partial class StockPage : Page
    {
        private readonly StockService _stockService = new StockService();
        private readonly List<string> _symbols = new List<string> { "AAPL", "TSLA", "AMZN" };

        public StockPage()
        {
            InitializeComponent();
            LoadStocks();
        }

        private async void LoadStocks()
        {
            StockList.Items.Clear();

            foreach (var symbol in _symbols)
            {
                string price = await _stockService.GetPrice(symbol);
                string display = price != null
                    ? $"{symbol}: {price} USD"
                    : $"{symbol}: Kurs konnte nicht geladen werden";

                StockList.Items.Add(display);
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadStocks();
        }
    }
}
