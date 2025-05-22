using System.Data.SQLite;

namespace MoneyMind
{
  public static class Database
  {
    private static SQLiteConnection _connection;

    public static SQLiteConnection Connection
    {
      get
      {
        if (_connection == null)
        {
          _connection = new SQLiteConnection("Data Source=users.db;Version=3;");
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
  }
}
