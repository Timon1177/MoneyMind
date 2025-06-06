using MoneyMind.Models;
using Xunit;

namespace MoneyMind.Tests
{
  public class IncomeEntryTests
  {
    [Fact]
    public void GetEntryType_ReturnsIncome()
    {
      var entry = new IncomeEntry();
      var result = entry.GetEntryType();
      Assert.Equal("Income", result);
    }

    [Fact]
    public void AmountFormatted_ReturnsCorrectFormat()
    {
      var entry = new IncomeEntry { Amount = 1234.567 };

      string result = new string(entry.AmountFormatted.Where(c => char.IsDigit(c) || c == '.' || c == '-').ToArray());

      Assert.Equal("1234.57", result);
    }


    [Fact]
    public void Category_SetAndGet_Works()
    {
      var entry = new IncomeEntry { Category = "Job" };
      Assert.Equal("Job", entry.Category);
    }

    [Fact]
    public void Id_SetAndGet_Works()
    {
      var entry = new IncomeEntry { Id = 99 };
      Assert.Equal(99, entry.Id);
    }

    [Fact]
    public void NegativeAmount_FormatsCorrectly()
    {
      var entry = new IncomeEntry { Amount = -42.4242 };
      Assert.Equal("-42.42", entry.AmountFormatted.Replace(",", "").Replace("'", ""));
    }
  }

  public class ExpenseEntryTests
  {
    [Fact]
    public void GetEntryType_ReturnsExpectedFormat()
    {
      var entry = new ExpenseEntry { Type = "Other" };
      Assert.Equal("Expense (Other)", entry.GetEntryType());
    }

    [Fact]
    public void GetEntryType_WithNullType_ReturnsEmpty()
    {
      var entry = new ExpenseEntry { Type = null };
      Assert.Equal("Expense ()", entry.GetEntryType());
    }

    [Fact]
    public void ExpenseEntry_CategoryAndAmount_AreSetCorrectly()
    {
      var entry = new ExpenseEntry { Category = "Food", Amount = 20.5 };
      Assert.Equal("Food", entry.Category);
      Assert.Equal(20.5, entry.Amount);
    }
  }

  
  public class DatabaseTests
  {
    [Fact]
    public void Connection_IsSingleton()
    {
      var conn1 = MoneyMind.Database.Connection;
      var conn2 = MoneyMind.Database.Connection;
      Assert.Same(conn1, conn2); 
    }
  }
}
