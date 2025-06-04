using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
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

        private void DeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            var result1 = MessageBox.Show("Are you sure you want to delete your account?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result1 != MessageBoxResult.Yes)
                return;

            var result2 = MessageBox.Show("This action cannot be undone. Do you really want to continue?", "Final Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result2 != MessageBoxResult.Yes)
                return;

            int userId = CurrentUser.UserID;

            using (var cmd = new SQLiteCommand("DELETE FROM users WHERE id = @id", Database.Connection))
            {
                cmd.Parameters.AddWithValue("@id", userId);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Your account has been deleted.", "Account Deleted", MessageBoxButton.OK, MessageBoxImage.Information);

            var login = new Login();
            login.Show();
            Window.GetWindow(this)?.Close();
        }
    }
}
