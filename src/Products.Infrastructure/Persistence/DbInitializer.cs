using System.Data;

namespace Products.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static void Initialize(IDbConnection connection)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Products (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Price REAL NOT NULL,
                Category TEXT NOT NULL,
                Stock INTEGER NOT NULL
            );";
            cmd.ExecuteNonQuery();

            using var check = connection.CreateCommand();
            check.CommandText = "SELECT COUNT(1) FROM Products";
            var count = Convert.ToInt32(check.ExecuteScalar());
            if (count == 0)
            {
                using var insert = connection.CreateCommand();
                insert.CommandText = @"
                INSERT INTO Products (Name, Price, Category, Stock) VALUES
                ('Manzana', 1.2, 'Alimentos', 100),
                ('Teclado', 45.5, 'Electrónica', 10);";
                insert.ExecuteNonQuery();
            }
        }
    }
}
