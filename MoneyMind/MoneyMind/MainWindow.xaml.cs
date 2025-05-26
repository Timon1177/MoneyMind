using System.Windows;
using System.Windows.Controls;

namespace MoneyMind
{
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      MainFrame.Navigate(new StartPage()); // ← Hier StartPage statt HomePage
    }

    private void NavigateHome_Click(object sender, RoutedEventArgs e)
    {
      MainFrame.Navigate(new StartPage()); // ← Hier auch
    }

    private void NavigateIncome_Click(object sender, RoutedEventArgs e)
    {
      MainFrame.Navigate(new IncomePage());
    }

    private void NavigateGoals_Click(object sender, RoutedEventArgs e)
    {
      MainFrame.Navigate(new SavingGoals());
    }

    private void NavigateUser_Click(object sender, RoutedEventArgs e)
    {
      MainFrame.Navigate(new UserPage());
    }

    private void ExitApp_Click(object sender, RoutedEventArgs e)
    {
      Database.Close();
      Application.Current.Shutdown();
    }
  }
}
