using System.Windows;
using System.Windows.Controls;

namespace MoneyMind
{
  public partial class StartPage : Page
  {
    public StartPage()
    {
      InitializeComponent();

      decimal currentBalance = GetCurrentBalance();
      SaldoText.Text = $"CHF {currentBalance:N2}";
    }

    private decimal GetCurrentBalance()
    {
      return 3250.00m;
    }
  }
}
