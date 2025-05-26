using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

      // Einnahmen laden
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

      // Ausgaben laden
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

      UpdateSums();
    }

    private void UpdateSums()
    {
      txtTotalIncome.Text = $"Gesamteinnahmen: {Incomes.Sum(i => i.Amount):0.00} CHF";
      double totalExpenses = FixedExpenses.Sum(e => e.Amount) + OtherExpenses.Sum(e => e.Amount);
      txtTotalExpense.Text = $"Gesamtausgaben: {totalExpenses:0.00} CHF";
    }

    private void AddIncome_Click(object sender, RoutedEventArgs e)
    {
      // Hier kannst du ein eigenes Eingabefeld bauen oder einen Dialog verwenden
      string category = "Lohn";
      double amount = 1000;

      var connection = Database.Connection;
      string insert = "INSERT INTO Income (Category, Amount, fk_userID) VALUES (@cat, @amt, @userID)";
      using var cmd = new SQLiteCommand(insert, connection);
      cmd.Parameters.AddWithValue("@cat", category);
      cmd.Parameters.AddWithValue("@amt", amount);
      cmd.Parameters.AddWithValue("@userID", CurrentUser.UserID);
      cmd.ExecuteNonQuery();

      LoadData();
    }

    private void AddExpense_Click(object sender, RoutedEventArgs e)
    {
      string category = "Miete";
      double amount = 800;
      string type = "Fixed";

      var connection = Database.Connection;
      string insert = "INSERT INTO Expenses (Category, Amount, Type, fk_userID) VALUES (@cat, @amt, @type, @userID)";
      using var cmd = new SQLiteCommand(insert, connection);
      cmd.Parameters.AddWithValue("@cat", category);
      cmd.Parameters.AddWithValue("@amt", amount);
      cmd.Parameters.AddWithValue("@type", type);
      cmd.Parameters.AddWithValue("@userID", CurrentUser.UserID);
      cmd.ExecuteNonQuery();

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
    public string Type { get; set; } // Nur für Expense
  }
}
