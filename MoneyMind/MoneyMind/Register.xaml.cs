using System;
using System.Data.SQLite;
using System.Windows;

namespace MoneyMind
{
    public partial class Register : Window
    {
        private string connectionString = "Data Source=users.db;Version=3;";

        public Register()
        {
            InitializeComponent();
        }

    private void BtnRegister_Click(object sender, RoutedEventArgs e)
    {
      string firstName = txtFirstName.Text;
      string lastName = txtLastName.Text;
      string email = txtEmail.Text;
      string username = txtUsername.Text;
      string password = txtPassword.Password;

      if (string.IsNullOrWhiteSpace(firstName) ||
          string.IsNullOrWhiteSpace(lastName) ||
          string.IsNullOrWhiteSpace(email) ||
          string.IsNullOrWhiteSpace(username) ||
          string.IsNullOrWhiteSpace(password))
      {
        MessageBox.Show("Please fill in all fields.");
        return;
      }

      var connection = Database.Connection;

      string checkQuery = "SELECT COUNT(*) FROM users WHERE username = @username";
      using (var checkCmd = new SQLiteCommand(checkQuery, connection))
      {
        checkCmd.Parameters.AddWithValue("@username", username);
        long userCount = (long)checkCmd.ExecuteScalar();

        if (userCount > 0)
        {
          MessageBox.Show("Username already exists.");
          return;
        }
      }

      string query = "INSERT INTO users (firstname, lastname, email, username, password) VALUES (@fn, @ln, @em, @us, @pw)";
      using (var command = new SQLiteCommand(query, connection))
      {
        command.Parameters.AddWithValue("@fn", firstName);
        command.Parameters.AddWithValue("@ln", lastName);
        command.Parameters.AddWithValue("@em", email);
        command.Parameters.AddWithValue("@us", username);
        command.Parameters.AddWithValue("@pw", password);

        try
        {
          command.ExecuteNonQuery();

          long userId = connection.LastInsertRowId;

          string insertBalanceQuery = @"
            INSERT INTO balance (amount, fk_userID)
            VALUES (0, @userId);";

          using (var balanceCmd = new SQLiteCommand(insertBalanceQuery, connection))
          {
            balanceCmd.Parameters.AddWithValue("@userId", userId);
            balanceCmd.ExecuteNonQuery();
          }
          MessageBox.Show("Registration successful!");
          new Login().Show();
          this.Close();
        }
        catch (Exception ex)
        {
          MessageBox.Show("An error occurred: " + ex.Message);
        }
      }
    }


    private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            new Login().Show();
            this.Close();
        }
    }
}
