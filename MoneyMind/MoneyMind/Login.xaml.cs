using System;
using System.Data.SQLite;
using System.Windows;

namespace MoneyMind
{
  public partial class Login : Window
  {
    private string connectionString = "Data Source=users.db;Version=3;";

    public Login()
    {
      InitializeComponent();
      InitializeDatabase();
    }

    private void InitializeDatabase()
    {
      using (var connection = new SQLiteConnection(connectionString))
      {
        connection.Open();
        string query = "CREATE TABLE IF NOT EXISTS users (id INTEGER PRIMARY KEY, username TEXT UNIQUE, password TEXT);";
        using (var command = new SQLiteCommand(query, connection))
        {
          command.ExecuteNonQuery();
        }
      }
    }

    private void BtnRegister_Click(object sender, RoutedEventArgs e)
    {
      string username = txtUsername.Text;
      string password = txtPassword.Password;

      using (var connection = new SQLiteConnection(connectionString))
      {
        connection.Open();
        string query = "INSERT INTO users (username, password) VALUES (@username, @password)";
        using (var command = new SQLiteCommand(query, connection))
        {
          command.Parameters.AddWithValue("@username", username);
          command.Parameters.AddWithValue("@password", password);
          try
          {
            command.ExecuteNonQuery();
            MessageBox.Show("Registrierung erfolgreich!");
          }
          catch (Exception ex)
          {
            MessageBox.Show("Fehler: " + ex.Message);
          }
        }
      }
    }

    private void BtnLogin_Click(object sender, RoutedEventArgs e)
    {
      string username = txtUsername.Text;
      string password = txtPassword.Password;

      using (var connection = new SQLiteConnection(connectionString))
      {
        connection.Open();
        string query = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password";
        using (var command = new SQLiteCommand(query, connection))
        {
          command.Parameters.AddWithValue("@username", username);
          command.Parameters.AddWithValue("@password", password);
          int count = Convert.ToInt32(command.ExecuteScalar());
          if (count > 0)
          {
            MessageBox.Show("Login erfolgreich!");
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
          }
          else
          {
            MessageBox.Show("Falsche Anmeldedaten!");
          }
        }
      }
    }
  }
}
