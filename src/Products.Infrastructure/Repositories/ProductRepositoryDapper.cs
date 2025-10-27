using Dapper;
using Products.Application.Abstractions.Interfaces;
using Products.Domain.Entities;
using Products.SharedKernel;
using System.Data;

public class ProductRepositoryDapper : IProductRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public ProductRepositoryDapper(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    private IDbConnection Connection => _connectionFactory.GetSharedConnection();

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var sql = "SELECT Id, Name, Price, Category, Stock FROM Products";
        return await Connection.QueryAsync<Product>(sql);
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var sql = "SELECT Id, Name, Price, Category, Stock FROM Products WHERE Id = @Id";
        return await Connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        var sql = @"
            INSERT INTO Products (Name, Price, Category, Stock)
            VALUES (@Name, @Price, @Category, @Stock);
            SELECT last_insert_rowid();";
        var id = await Connection.ExecuteScalarAsync<long>(sql, new
        {
            product.Name,
            product.Price,
            product.Category,
            product.Stock
        });
        return (int)id;
    }

    public async Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        var sql = @"
            UPDATE Products
            SET Name = @Name, Price = @Price, Category = @Category, Stock = @Stock
            WHERE Id = @Id";
        var rows = await Connection.ExecuteAsync(sql, new
        {
            product.Name,
            product.Price,
            product.Category,
            product.Stock,
            product.Id
        });
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var sql = "DELETE FROM Products WHERE Id = @Id";
        var rows = await Connection.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }

    public async Task<IEnumerable<Product>> GetFilteredAsync(string categoria, decimal precioMin, CancellationToken cancellationToken = default)
    {
        var sql = @"
        SELECT Id, Name, Price, Category, Stock
        FROM Products
        WHERE Category = @Categoria AND Price >= @PrecioMin";
        return await Connection.QueryAsync<Product>(sql, new { Categoria = categoria, PrecioMin = precioMin });
    }
}
