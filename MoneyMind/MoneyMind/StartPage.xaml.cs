using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
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
    }

    private decimal GetCurrentBalance()
    {
      var connection = Database.Connection;
      string query = "SELECT amount FROM balance WHERE fk_userID = @userID";

      using (var command = new SQLiteCommand(query, connection))
      {
        command.Parameters.AddWithValue("@userID", CurrentUser.UserID);

        object result = command.ExecuteScalar();
        if (result != null && decimal.TryParse(result.ToString(), out decimal balance))
        {
          return balance;
        }
        else
        {
          return 0;
        }
      }
    }




  }
}
