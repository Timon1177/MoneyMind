using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoneyMind
{
    /// <summary>
    /// Interaktionslogik für SavingGoals.xaml
    /// </summary>
    public partial class SavingGoals : Page
    {
        public SavingGoals()
        {
            InitializeComponent();
            LoadSavingGoals();
        }

        private class SavingGoalsInfo
        {
          public string GoalName { get; set; }
          public decimal TargetAmount { get; set; }
          public decimal CurrentAmount { get; set; }
          public DateTime Deadline { get; set; }
        }

        private void LoadSavingGoals()
        {
          
        }
    }
}
