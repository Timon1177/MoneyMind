using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using MoneyMind.Models;

namespace MoneyMind
{
  public static class Database
  {
    private static SQLiteConnection _connection;
    private static string _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.db");

    public static SQLiteConnection Connection
    {
      get
      {
        if (_connection == null)
        {
          _connection = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
          _connection.Open();
        }
        return _connection;
      }
    }

    public static void Close()
    {
      _connection?.Close();
      _connection = null;
    }

    public static decimal GetUserBalance(int userId)
    {
      using var cmd = new SQLiteCommand("SELECT Balance FROM users WHERE id = @id", Connection);
      cmd.Parameters.AddWithValue("@id", userId);
      var result = cmd.ExecuteScalar();
      return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
    }

    public static void UpdateBalance(int userId, decimal newBalance)
    {
      using var cmd = new SQLiteCommand("UPDATE users SET Balance = @balance WHERE id = @id", Connection);
      cmd.Parameters.AddWithValue("@balance", newBalance);
      cmd.Parameters.AddWithValue("@id", userId);
      cmd.ExecuteNonQuery();
    }

    public static decimal GetTotalIncome(int userId)
    {
      using var cmd = new SQLiteCommand("SELECT SUM(Amount) FROM Income WHERE fk_userID = @UserId", Connection);
      cmd.Parameters.AddWithValue("@UserId", userId);
      var result = cmd.ExecuteScalar();
      return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
    }

    public static decimal GetTotalExpense(int userId)
    {
      using var cmd = new SQLiteCommand("SELECT SUM(Amount) FROM Expenses WHERE fk_userID = @UserId", Connection);
      cmd.Parameters.AddWithValue("@UserId", userId);
      var result = cmd.ExecuteScalar();
      return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
    }

    public static void InsertStockPurchase(StockPurchase purchase)
    {
      using var cmd = new SQLiteCommand(@"
                INSERT INTO StockPurchases (UserId, Symbol, Quantity, PurchasePrice, PurchaseDate)
                VALUES (@UserId, @Symbol, @Quantity, @Price, @Date);", Connection);
      cmd.Parameters.AddWithValue("@UserId", purchase.UserId);
      cmd.Parameters.AddWithValue("@Symbol", purchase.Symbol);
      cmd.Parameters.AddWithValue("@Quantity", purchase.Quantity);
      cmd.Parameters.AddWithValue("@Price", purchase.PurchasePrice);
      cmd.Parameters.AddWithValue("@Date", purchase.PurchaseDate.ToString("yyyy-MM-dd HH:mm:ss"));
      cmd.ExecuteNonQuery();
    }

    public static List<StockPurchase> GetStockPurchasesByUser(int userId)
    {
      var list = new List<StockPurchase>();
      using var cmd = new SQLiteCommand("SELECT * FROM StockPurchases WHERE UserId = @UserId", Connection);
      cmd.Parameters.AddWithValue("@UserId", userId);
      using var reader = cmd.ExecuteReader();

      while (reader.Read())
      {
        list.Add(new StockPurchase
        {
          Id = Convert.ToInt32(reader["Id"]),
          UserId = Convert.ToInt32(reader["UserId"]),
          Symbol = reader["Symbol"].ToString() ?? "",
          Quantity = Convert.ToInt32(reader["Quantity"]),
          PurchasePrice = Convert.ToDecimal(reader["PurchasePrice"]),
          PurchaseDate = DateTime.Parse(reader["PurchaseDate"].ToString() ?? "")
        });
      }

      return list;
    }

    public static void SaveFeedback(string title, string message, string? email)
    {
      using var cmd = new SQLiteCommand("INSERT INTO Feedback (Title, Message, Email) VALUES (@title, @message, @email)", Connection);
      cmd.Parameters.AddWithValue("@title", title);
      cmd.Parameters.AddWithValue("@message", message);
      cmd.Parameters.AddWithValue("@email", string.IsNullOrWhiteSpace(email) ? DBNull.Value : email);
      cmd.ExecuteNonQuery();
    }

    public static void insertTestData()
    {
      string insertPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sqlscripts", "insert.sql");
      if (File.Exists(insertPath))
      {
        ExecuteSqlScript(insertPath);
        System.Diagnostics.Debug.WriteLine("insert.sql executed.");
      }
    }

    public static void InitializeDatabase()
    {
      try
      {
        if (!File.Exists(_dbPath))
        {
          SQLiteConnection.CreateFile(_dbPath);
          Console.WriteLine("users.db created at: " + _dbPath);
        }
        else
        {
          Console.WriteLine("users.db already exists.");
        }

        string createPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sqlscripts", "create.sql");
        if (!File.Exists(createPath))
        {
          Console.WriteLine("create.sql not found: " + createPath);
          return;
        }
        else
        {
          ExecuteSqlScript(createPath);
          Console.WriteLine("create.sql executed.");
        }

        string updatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sqlscripts", "update.sql");
        if (File.Exists(updatePath))
        {
          ExecuteSqlScript(updatePath);
          Console.WriteLine("update.sql executed.");
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error:\n" + ex.Message);
      }
    }

    private static void ExecuteSqlScript(string path)
    {
      using var connection = new SQLiteConnection($"Data Source={_dbPath};");
      connection.Open();
      string script = File.ReadAllText(path);

      foreach (var commandText in script.Split(';'))
      {
        string trimmed = commandText.Trim();
        if (!string.IsNullOrWhiteSpace(trimmed))
        {
          using var command = new SQLiteCommand(trimmed, connection);
          command.ExecuteNonQuery();
        }
      }
    }

    public static void InsertSampleData()
    {
      var samplePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sqlscripts", "insert_sample.sql");
      if (File.Exists(samplePath))
      {
        ExecuteSqlScript(samplePath);
      }
    }
  }
}
