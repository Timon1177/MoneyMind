using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Controls;
using static MoneyMind.Login;
using System.Windows.Media;
using System.Windows; 


namespace MoneyMind
{
  public partial class SavingGoals : Page
  {
    public SavingGoals()
    {
      InitializeComponent();
      LoadSavingGoals();
    }

    public class SavingGoalsInfo
    {
      public string GoalName { get; set; }
      public decimal TargetAmount { get; set; }
      public decimal CurrentAmount { get; set; }
      public DateTime Deadline { get; set; }
    }

    private void LoadSavingGoals()
    {
      List<SavingGoalsInfo> goals = GetSavingGoals();

      if (goals.Count == 0)
      {
        goalsPanel.Children.Add(new TextBlock
        {
          Text = "Keine Sparziele gefunden.",
          FontSize = 18,
          Foreground = System.Windows.Media.Brushes.Gray
        });
        return;
      }

      foreach (var goal in goals)
      {
        var border = new Border
        {
          Background = Brushes.White,
          BorderBrush = Brushes.Goldenrod,
          BorderThickness = new System.Windows.Thickness(1),
          CornerRadius = new CornerRadius(10),
          Margin = new System.Windows.Thickness(0, 10, 0, 0),
          Padding = new System.Windows.Thickness(10)
        };

        var stack = new StackPanel();

        stack.Children.Add(new TextBlock
        {
          Text = $"🎯 Ziel: {goal.GoalName}",
          FontSize = 16,
          FontWeight = FontWeights.Bold,
          Foreground = Brushes.DarkSlateGray
        });

        stack.Children.Add(new TextBlock
        {
          Text = $"💰 Zielbetrag: {goal.TargetAmount:C}",
          FontSize = 14
        });

        stack.Children.Add(new TextBlock
        {
          Text = $"📅 Deadline: {goal.Deadline:dd.MM.yyyy}",
          FontSize = 14
        });

        border.Child = stack;
        goalsPanel.Children.Add(border);
      }
    }

    private List<SavingGoalsInfo> GetSavingGoals()
    {
      var result = new List<SavingGoalsInfo>();
      var connection = Database.Connection;
      string query = "SELECT GoalName, TargetAmount, DeadLine FROM SavingGoals WHERE fk_userId = @userID";

      using (var command = new SQLiteCommand(query, connection))
      {
        command.Parameters.AddWithValue("@userID", CurrentUser.UserID);

        using (var reader = command.ExecuteReader())
        {
          while (reader.Read())
          {
            result.Add(new SavingGoalsInfo
            {
              GoalName = reader.GetString(0),
              TargetAmount = reader.GetDecimal(1),
              Deadline = reader.GetDateTime(2)
            });
          }
        }
      }

      return result;
    }
  }
}
