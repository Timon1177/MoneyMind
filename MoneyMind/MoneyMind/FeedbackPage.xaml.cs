using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace MoneyMind
{
    public partial class FeedbackPage : Page
    {
        public FeedbackPage()
        {
            InitializeComponent();
        }

        private void SubmitFeedback(object sender, RoutedEventArgs e)
        {
            string title = TitleInput.Text.Trim();
            string message = MessageInput.Text.Trim();
            string email = EmailInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show("Please enter at least a title and a message.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using var conn = Database.Connection;
                using var cmd = new SQLiteCommand("INSERT INTO Feedback (Title, Message, Email) VALUES (@title, @message, @email)", conn);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@message", message);
                cmd.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(email) ? DBNull.Value : email);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Thank you for your feedback!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                TitleInput.Text = "";
                MessageInput.Text = "";
                EmailInput.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving your feedback:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}