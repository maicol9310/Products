using System.Data;
using Microsoft.Data.Sqlite;
using Products.SharedKernel;

namespace Products.Infrastructure.Connection
{
    public class SqliteConnectionFactory : IConnectionFactory, IDisposable
    {
        private readonly string _connectionString;
        private readonly IDbConnection _sharedConnection;

        public SqliteConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
            _sharedConnection = new SqliteConnection(_connectionString);
            _sharedConnection.Open();
        }

        public IDbConnection CreateConnection()
        {
            var cn = new SqliteConnection(_connectionString);
            cn.Open();
            return cn;
        }

        public IDbConnection GetSharedConnection() => _sharedConnection;

        public void Dispose()
        {
            try { _sharedConnection?.Dispose(); } catch { }
        }
    }
}
