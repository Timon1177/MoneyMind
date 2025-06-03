using System.Windows;
using System.Windows.Controls;

namespace MoneyMind
{
    public partial class MainWindow : Window
    {
        private StockPage? _stockPageInstance;

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new StartPage());
        }

        private void NavigateHome_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new StartPage());
        }

        private void NavigateIncome_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new IncomePage());
        }

        private void NavigateGoals_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SavingGoals());
        }

        private void NavigateStocks_Click(object sender, RoutedEventArgs e)
        {
            if (_stockPageInstance == null)
            {
                int userId = 1;
                decimal balance = Database.GetUserBalance(userId);

                _stockPageInstance = new StockPage(userId, balance);
                _stockPageInstance.StartChartUpdates();
            }

            MainFrame.Navigate(_stockPageInstance);
        }

        private void NavigateUser_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UserPage());
        }

        private void ExitApp_Click(object sender, RoutedEventArgs e)
        {
            Database.Close();
            Application.Current.Shutdown();
        }
    }
}
