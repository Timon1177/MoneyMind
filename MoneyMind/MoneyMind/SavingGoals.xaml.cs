using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Controls;
using static MoneyMind.Login;
using System.Windows.Media;
using System.Windows;
using System.Globalization;

namespace MoneyMind
{
  public partial class SavingGoals : Page
  {
    public SavingGoals()
    {
      InitializeComponent();
      LoadSavingGoals();
    }

    private void ToggleSavingGoalForm_Click(object sender, RoutedEventArgs e)
    {
      SavingGoalForm.Visibility = SavingGoalForm.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
    }

    private void CancelSavingGoalForm_Click(object sender, RoutedEventArgs e)
    {
      SavingGoalNameInput.Text = "";
      SavingGoalAmountInput.Text = "";
      SavingGoalDeadlineInput.Text = "";
      SavingGoalForm.Visibility = Visibility.Collapsed;
    }

    private void SaveSavingGoal_Click(object sender, RoutedEventArgs e)
    {
      string name = SavingGoalNameInput.Text.Trim();
      string deadline = SavingGoalDeadlineInput.Text.Trim();

      if (!double.TryParse(SavingGoalAmountInput.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double amount)
          || amount <= 0 || string.IsNullOrWhiteSpace(name) || string.IsNullOrEmpty(deadline))
      {
        MessageBox.Show("Please enter a valid goal name, amount, and deadline.");
        return;
      }

      var connection = Database.Connection;
      string insert = "INSERT INTO SavingGoals (GoalName, TargetAmount, DeadLine, fk_userID) VALUES (@name, @Tamt, @deadline, @userID)";
      using var cmd = new SQLiteCommand(insert, connection);
      cmd.Parameters.AddWithValue("@name", name);
      cmd.Parameters.AddWithValue("@Tamt", amount);
      cmd.Parameters.AddWithValue("@deadline", deadline);
      cmd.Parameters.AddWithValue("@userID", CurrentUser.UserID);
      cmd.ExecuteNonQuery();

      CancelSavingGoalForm_Click(null, null);
      LoadSavingGoals();
    }

    public class SavingGoalsInfo
    {
      public int GoalID { get; set; }
      public string GoalName { get; set; }
      public decimal TargetAmount { get; set; }
      public DateTime Deadline { get; set; }
    }

    private void LoadSavingGoals()
    {
      goalsPanel.Children.Clear();
      List<SavingGoalsInfo> goals = GetSavingGoals();

      if (goals.Count == 0)
      {
        goalsPanel.Children.Add(new TextBlock
        {
          Text = "No saving goals found.",
          FontSize = 18,
          Foreground = Brushes.Gray
        });
        return;
      }

      decimal balance = 0;
      var connection = Database.Connection;

      string query = "SELECT amount FROM Balance WHERE fk_userID = @userID";
      using (var cmd = new SQLiteCommand(query, connection))
      {
        cmd.Parameters.AddWithValue("@userID", CurrentUser.UserID);
        object result = cmd.ExecuteScalar();
        if (result != null && decimal.TryParse(result.ToString(), out var value))
        {
          balance = value;
        }
      }

      foreach (var goal in goals)
      {
        var border = new Border
        {
          Background = Brushes.White,
          BorderBrush = Brushes.Goldenrod,
          BorderThickness = new Thickness(1),
          CornerRadius = new CornerRadius(10),
          Margin = new Thickness(0, 10, 0, 0),
          Padding = new Thickness(10)
        };

        var stack = new StackPanel();

        stack.Children.Add(new TextBlock
        {
          Text = $"🎯 Goal: {goal.GoalName}",
          FontSize = 16,
          FontWeight = FontWeights.Bold,
          Foreground = Brushes.DarkSlateGray
        });

        stack.Children.Add(new TextBlock
        {
          Text = $"💰 Target Amount: {goal.TargetAmount:C}",
          FontSize = 14
        });

        stack.Children.Add(new TextBlock
        {
          Text = $"📅 Deadline: {goal.Deadline:dd.MM.yyyy}",
          FontSize = 14
        });

        stack.Children.Add(new TextBlock
        {
          Text = $"✅ Progress: {(balance / goal.TargetAmount * 100):N2} %",
          FontSize = 14,
        });

        stack.Children.Add(new TextBlock
        {
          Text = $"⏳ Days remaining: {(goal.Deadline - DateTime.Now).Days} days",
          FontSize = 14,
          Foreground = (goal.Deadline < DateTime.Now) ? Brushes.Red : Brushes.Green
        });

        var deleteButton = new Button
        {
          Content = "🗑️ Delete",
          Background = Brushes.IndianRed,
          Foreground = Brushes.White,
          Margin = new Thickness(0, 10, 0, 0),
          Width = 100,
          Tag = goal.GoalID
        };
        deleteButton.Click += DeleteSavingGoal_Click;

        stack.Children.Add(deleteButton);
        border.Child = stack;
        goalsPanel.Children.Add(border);
      }
    }

    private List<SavingGoalsInfo> GetSavingGoals()
    {
      var result = new List<SavingGoalsInfo>();
      var connection = Database.Connection;
      string query = "SELECT GoalID, GoalName, TargetAmount, DeadLine FROM SavingGoals WHERE fk_userID = @userID";

      using (var command = new SQLiteCommand(query, connection))
      {
        command.Parameters.AddWithValue("@userID", CurrentUser.UserID);

        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            result.Add(new SavingGoalsInfo
            {
              GoalID = reader.GetInt32(0),
              GoalName = reader.GetString(1),
              TargetAmount = reader.GetDecimal(2),
              Deadline = reader.GetDateTime(3)
            });
          }
        }
      }

      return result;
    }

    private void DeleteSavingGoal_Click(object sender, RoutedEventArgs e)
    {
      if (sender is Button btn && btn.Tag is int goalID)
      {
        var connection = Database.Connection;
        string delete = "DELETE FROM SavingGoals WHERE GoalID = @id AND fk_userID = @userID";
        using var cmd = new SQLiteCommand(delete, connection);
        cmd.Parameters.AddWithValue("@id", goalID);
        cmd.Parameters.AddWithValue("@userID", CurrentUser.UserID);
        cmd.ExecuteNonQuery();

        LoadSavingGoals();
      }
    }
  }
}
