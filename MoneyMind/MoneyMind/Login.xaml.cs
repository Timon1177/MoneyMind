﻿using System;
using System.Data.SQLite;
using System.Windows;

namespace MoneyMind
{
  public partial class Login : Window
  {
    public Login()
    {
      InitializeComponent();
      InitializeDatabase();
    }

    public static class CurrentUser
    {
      public static int UserID { get; set; }
    }


    private void InitializeDatabase()
    {
      var connection = Database.Connection;

      string createUsersTable = @"
    CREATE TABLE IF NOT EXISTS users (
        id INTEGER PRIMARY KEY,
        firstname TEXT,
        lastname TEXT,
        email TEXT,
        username TEXT UNIQUE,
        password TEXT
    );";

      string createBalanceTable = @"
    CREATE TABLE IF NOT EXISTS balance (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        fk_userID INTEGER NOT NULL,
        amount REAL NOT NULL DEFAULT 0,
        FOREIGN KEY (fk_userID) REFERENCES users(id)
    );";


      using (var command = new SQLiteCommand(createUsersTable, connection))
      {
        command.ExecuteNonQuery();
      }

      using (var command = new SQLiteCommand(createBalanceTable, connection))
      {
        command.ExecuteNonQuery();
      }
    }


    private void BtnRegister_Click(object sender, RoutedEventArgs e)
    {
      Register registerWindow = new Register();
      registerWindow.Show();
      this.Close();
    }

    private void BtnLogin_Click(object sender, RoutedEventArgs e)
    {
      string username = txtUsername.Text;
      string password = txtPassword.Password;

      var connection = Database.Connection;

      string query = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password";
      using (var command = new SQLiteCommand(query, connection))
      {
        command.Parameters.AddWithValue("@username", username);
        command.Parameters.AddWithValue("@password", password);
        int count = Convert.ToInt32(command.ExecuteScalar());

        if (count > 0)
        {
          string getIdQuery = "SELECT id FROM users WHERE username = @username AND password = @password ";
          using (var idCommand = new SQLiteCommand(getIdQuery, connection))
          {
            idCommand.Parameters.AddWithValue("@username", username);
            idCommand.Parameters.AddWithValue("@password", password);
            object result = idCommand.ExecuteScalar();
            if (result != null && int.TryParse(result.ToString(), out int userId))
            {
              CurrentUser.UserID = userId;
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

    private void ToggleWindowMode_Click(object sender, RoutedEventArgs e)
    {
      if (this.WindowState == WindowState.Maximized && this.WindowStyle == WindowStyle.None)
      {
        this.WindowStyle = WindowStyle.SingleBorderWindow;
        this.WindowState = WindowState.Normal;
        FullscreenToggleButton.Content = "🗗";
      }
      else
      {
        this.WindowStyle = WindowStyle.None;
        this.WindowState = WindowState.Maximized;
        FullscreenToggleButton.Content = "⤢";
      }
    }
  }
}
