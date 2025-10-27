using Products.Domain.Entities;

namespace Products.Application.Abstractions.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(Product product, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetFilteredAsync(string categoria, decimal precioMin, CancellationToken cancellationToken = default);
    }
}
