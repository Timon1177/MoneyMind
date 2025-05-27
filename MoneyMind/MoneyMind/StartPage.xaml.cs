using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static MoneyMind.Login;

namespace MoneyMind
{
  public partial class StartPage : Page
  {
    public StartPage()
    {
      InitializeComponent();

      decimal currentBalance = GetCurrentBalance();
      SaldoText.Text = $"CHF {currentBalance:N2}";

      if (currentBalance < 0)
      {
        SaldoText.Foreground = new SolidColorBrush(Colors.Red);
      }
      else
      {
        SaldoText.Foreground = new SolidColorBrush(Color.FromRgb(0x34, 0xA8, 0x5F)); // schönes Grün
      }
    }

    private decimal GetCurrentBalance()
    {
      var connection = Database.Connection;

      decimal income = 0;
      decimal expenses = 0;

      string incomeQuery = "SELECT IFNULL(SUM(Amount), 0) FROM Income WHERE fk_userID = @userID";
      using (var cmd = new SQLiteCommand(incomeQuery, connection))
      {
        cmd.Parameters.AddWithValue("@userID", CurrentUser.UserID);
        object result = cmd.ExecuteScalar();
        if (result != null && decimal.TryParse(result.ToString(), out var value))
          income = value;
      }

      string expenseQuery = "SELECT IFNULL(SUM(Amount), 0) FROM Expenses WHERE fk_userID = @userID";
      using (var cmd = new SQLiteCommand(expenseQuery, connection))
      {
        cmd.Parameters.AddWithValue("@userID", CurrentUser.UserID);
        object result = cmd.ExecuteScalar();
        if (result != null && decimal.TryParse(result.ToString(), out var value))
          expenses = value;
      }

      return income - expenses;
    }
  }
}
