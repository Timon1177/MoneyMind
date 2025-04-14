using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoneyMind
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
        public MainWindow()
        {
            InitializeComponent();

            decimal currentBalance = GetCurrentBalance();
            SaldoText.Text = $"CHF {currentBalance:N2}";
        }

        private void ToggleWindowMode_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized && this.WindowStyle == WindowStyle.None)
            {
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Normal;
                FullscreenToggleButton.Content = "🗗";
            }
            else
            {
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
                FullscreenToggleButton.Content = "⤢";
            }
        }

        private void OpenTracking_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Navigating to I & E");
        }

        private void OpenGoals_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Navigating to S G");
        }

        private void OpenUser_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Navigating to U M");
        }
        private decimal GetCurrentBalance()
        {
            // Timon hier bitte echter Saldo von Datenbank
            return 3250.00m; //Temporär
        }
  }
}