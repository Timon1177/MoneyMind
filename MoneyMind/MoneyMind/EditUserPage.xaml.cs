using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using static MoneyMind.Login;

namespace MoneyMind
{
    public partial class EditUserPage : Page
    {
        public EditUserPage()
        {
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            var connection = Database.Connection;
            string query = "SELECT firstname, lastname, email, username FROM users WHERE id = @userID";

            using var command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("@userID", CurrentUser.UserID);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                FirstNameInput.Text = reader["firstname"]?.ToString() ?? "";
                LastNameInput.Text = reader["lastname"]?.ToString() ?? "";
                EmailInput.Text = reader["email"]?.ToString() ?? "";
                UsernameInput.Text = reader["username"]?.ToString() ?? "";
            }
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            string firstname = FirstNameInput.Text.Trim();
            string lastname = LastNameInput.Text.Trim();
            string email = EmailInput.Text.Trim();
            string username = UsernameInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(lastname) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using var cmd = new SQLiteCommand("UPDATE users SET firstname = @firstname, lastname = @lastname, email = @email, username = @username WHERE id = @id", Database.Connection);
                cmd.Parameters.AddWithValue("@firstname", firstname);
                cmd.Parameters.AddWithValue("@lastname", lastname);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@id", CurrentUser.UserID);
                cmd.ExecuteNonQuery();

                MessageBox.Show("User data updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                NavigationService?.Navigate(new UserPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating user data:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new UserPage());
        }
    }
}
