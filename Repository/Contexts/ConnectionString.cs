using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Collections.Concurrent;

namespace Repository.Contexts
{
    public static class ConnectionString
    {

        public static MySqlServerVersion s_ServerVersion = new(new Version(8, 0, 34));

        private static  ConcurrentDictionary<string, string> s_connectionStrings = new ConcurrentDictionary<string, string>();
        public static Action s_ReloadConnectionString { get; set; } = null!;

        public static string GetConnectionString(string dbName)
        {
            if (s_connectionStrings.TryGetValue(dbName, out var connectionString))
            {
                return connectionString;
            }

            try
            {
                s_ReloadConnectionString?.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reloading connection strings: {ex.Message}");
            }

            if (s_connectionStrings.TryGetValue(dbName, out connectionString))
            {
                return connectionString;
            }

            return string.Empty;
        }

        public static void Load(string dbName, string connectionString)
        {
            var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);
            s_connectionStrings[dbName] = connectionStringBuilder.ToString();
        }
    }
}
