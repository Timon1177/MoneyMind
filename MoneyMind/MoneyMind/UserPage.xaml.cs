using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static MoneyMind.Login;

namespace MoneyMind
{
  public partial class UserPage : Page
  {
    public UserPage()
    {
      InitializeComponent();
      LoadUserInfo();
    }

    public class UserInfo
    {
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string Email { get; set; }
      public string Username { get; set; }
    }

    private void LoadUserInfo()
    {
      var user = GetUserInfo();

      if (user == null)
      {
        firstname.Text = "Unknown";
        lastname.Text = "Unknown";
        email.Text = "Unknown";
        username.Text = "Unknown";
      }
      else 
      {
        firstname.Text = user.FirstName;
        lastname.Text = user.LastName; 
        email.Text = user.Email;
        username.Text = user.Username;
      }
    }

    private UserInfo GetUserInfo()
    {
      var connection = Database.Connection;
      string query = "SELECT firstname, lastname, email, username FROM users WHERE id = @userID";

      using (var command = new SQLiteCommand(query, connection))
      {
        command.Parameters.AddWithValue("@userID", CurrentUser.UserID);

        using (var reader = command.ExecuteReader())
        {
          if (reader.Read())
          {
            return new UserInfo
            {
              FirstName = reader.GetString(0),
              LastName = reader.GetString(1),
              Email = reader.GetString(2),
              Username = reader.GetString(3)
            };
          }
        }
      }
      return null;
    }
  }
}
