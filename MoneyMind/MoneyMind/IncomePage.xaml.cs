using MoneyMind;
using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;
using static MoneyMind.Login;


namespace MoneyMind
{
    public partial class IncomePage : Page
    {

        public ObservableCollection<Entry> Incomes { get; set; } = new();
        public ObservableCollection<Entry> FixedExpenses { get; set; } = new();
        public ObservableCollection<Entry> OtherExpenses { get; set; } = new();


        public IncomePage()
        {
            InitializeComponent();
            DataContext = this;
            LoadData();
        }

        private void LoadData()
        {
            Incomes.Clear();
            FixedExpenses.Clear();
            OtherExpenses.Clear();

            var connection = Database.Connection;

            string incomeQuery = "SELECT * FROM Income WHERE fk_userID = @userID";
            using (var cmd = new SQLiteCommand(incomeQuery, connection))
            {
                cmd.Parameters.AddWithValue("@userID", CurrentUser.UserID);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Incomes.Add(new Entry
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Category = reader["Category"].ToString(),
                        Amount = Convert.ToDouble(reader["Amount"])
                    });
                }
            }

            string expenseQuery = "SELECT * FROM Expenses WHERE fk_userID = @userID";
            using (var cmd = new SQLiteCommand(expenseQuery, connection))
            {
                cmd.Parameters.AddWithValue("@userID", CurrentUser.UserID);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var entry = new Entry
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Category = reader["Category"].ToString(),
                        Amount = Convert.ToDouble(reader["Amount"]),
                        Type = reader["Type"].ToString()
                    };

                    if (entry.Type == "Fixed")
                        FixedExpenses.Add(entry);
                    else
                        OtherExpenses.Add(entry);
                }
            }
            decimal balance = StartPage.GetCurrentBalance();

            UpdateSums();
        }

        private void UpdateSums()
        {
            txtTotalIncome.Text = $"Total income: {Incomes.Sum(i => i.Amount):N2} CHF";
            double totalExpenses = FixedExpenses.Sum(e => e.Amount) + OtherExpenses.Sum(e => e.Amount);
            txtTotalExpense.Text = $"Total expenses: {totalExpenses:N2} CHF";
        }

        private void ToggleIncomeForm_Click(object sender, RoutedEventArgs e)
        {
            IncomeForm.Visibility = IncomeForm.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void ToggleFixedForm_Click(object sender, RoutedEventArgs e)
        {
            FixedForm.Visibility = FixedForm.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void ToggleOtherForm_Click(object sender, RoutedEventArgs e)
        {
            OtherForm.Visibility = OtherForm.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void CancelIncomeForm_Click(object sender, RoutedEventArgs e)
        {
            IncomeCategoryInput.Text = "";
            IncomeAmountInput.Text = "";
            IncomeForm.Visibility = Visibility.Collapsed;
        }

        private void CancelFixedForm_Click(object sender, RoutedEventArgs e)
        {
            FixedCategoryInput.Text = "";
            FixedAmountInput.Text = "";
            FixedForm.Visibility = Visibility.Collapsed;
        }

        private void CancelOtherForm_Click(object sender, RoutedEventArgs e)
        {
            OtherCategoryInput.Text = "";
            OtherAmountInput.Text = "";
            OtherForm.Visibility = Visibility.Collapsed;
        }

        private void SaveIncome_Click(object sender, RoutedEventArgs e)
        {
            string category = IncomeCategoryInput.Text.Trim();
            if (!double.TryParse(IncomeAmountInput.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double amount) || amount <= 0 || string.IsNullOrWhiteSpace(category))
            {
                MessageBox.Show("Please enter a valid category and amount.");
                return;
            }

            amount = Math.Round(amount, 2);

            var connection = Database.Connection;
            string insert = "INSERT INTO Income (Category, Amount, fk_userID) VALUES (@cat, @amt, @userID)";
            using var cmd = new SQLiteCommand(insert, connection);
            cmd.Parameters.AddWithValue("@cat", category);
            cmd.Parameters.AddWithValue("@amt", amount);
            cmd.Parameters.AddWithValue("@userID", CurrentUser.UserID);
            cmd.ExecuteNonQuery();

            CancelIncomeForm_Click(null, null);
            LoadData();
        }

        private void SaveFixed_Click(object sender, RoutedEventArgs e)
        {
            string category = FixedCategoryInput.Text.Trim();
            if (!double.TryParse(FixedAmountInput.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double amount) || amount <= 0 || string.IsNullOrWhiteSpace(category))
            {
                MessageBox.Show("Please enter a valid category and amount.");
                return;
            }

            amount = Math.Round(amount, 2);

            var connection = Database.Connection;
            string insert = "INSERT INTO Expenses (Category, Amount, Type, fk_userID) VALUES (@cat, @amt, 'Fixed', @userID)";
            using var cmd = new SQLiteCommand(insert, connection);
            cmd.Parameters.AddWithValue("@cat", category);
            cmd.Parameters.AddWithValue("@amt", amount);
            cmd.Parameters.AddWithValue("@userID", CurrentUser.UserID);
            cmd.ExecuteNonQuery();

            CancelFixedForm_Click(null, null);
            LoadData();
        }

        private void SaveOther_Click(object sender, RoutedEventArgs e)
        {
            string category = OtherCategoryInput.Text.Trim();
            if (!double.TryParse(OtherAmountInput.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double amount) || amount <= 0 || string.IsNullOrWhiteSpace(category))
            {
                MessageBox.Show("Please enter a valid category and amount.");
                return;
            }

            amount = Math.Round(amount, 2);

            var connection = Database.Connection;
            string insert = "INSERT INTO Expenses (Category, Amount, Type, fk_userID) VALUES (@cat, @amt, 'Other', @userID)";
            using var cmd = new SQLiteCommand(insert, connection);
            cmd.Parameters.AddWithValue("@cat", category);
            cmd.Parameters.AddWithValue("@amt", amount);
            cmd.Parameters.AddWithValue("@userID", CurrentUser.UserID);
            cmd.ExecuteNonQuery();

            CancelOtherForm_Click(null, null);
            LoadData();
        }

        private void DeleteIncome_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                var connection = Database.Connection;
                string delete = "DELETE FROM Income WHERE Id = @id AND fk_userID = @userID";
                using var cmd = new SQLiteCommand(delete, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@userID", CurrentUser.UserID);
                cmd.ExecuteNonQuery();

                LoadData();
            }
        }

        private void DeleteFixedExpense_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                DeleteExpense(id);
            }
        }

        private void DeleteOtherExpense_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                DeleteExpense(id);
            }
        }

        private void DeleteExpense(int id)
        {
            var connection = Database.Connection;
            string delete = "DELETE FROM Expenses WHERE Id = @id AND fk_userID = @userID";
            using var cmd = new SQLiteCommand(delete, connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@userID", CurrentUser.UserID);
            cmd.ExecuteNonQuery();

            LoadData();
        }
    }

    public class Entry
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public double Amount { get; set; }
        public string AmountFormatted => Amount.ToString("N2");
        public string Type { get; set; } // Nur Expense
    }
}
