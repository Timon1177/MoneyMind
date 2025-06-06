using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyMind.Models
{
  public abstract class EntryBase
  {
    public int Id { get; set; }
    public string Category { get; set; }
    public double Amount { get; set; }
    public string AmountFormatted => Amount.ToString("N2");

    public abstract string GetEntryType();
  }

  public class IncomeEntry : EntryBase
  {
    public override string GetEntryType() => "Income";
  }

  public class ExpenseEntry : EntryBase
  {
    public string Type { get; set; }
    public override string GetEntryType() => $"Expense ({Type})";
  }
}

