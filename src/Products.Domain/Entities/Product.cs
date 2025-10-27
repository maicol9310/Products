using Products.SharedKernel;

namespace Products.Domain.Entities
{
    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public string Category { get; private set; } = string.Empty;
        public int Stock { get; private set; }

        private Product() { }

        public Product(string name, decimal price, string category, int stock)
        {
            EnsureIsValid(name, price, category, stock);
            Name = name.Trim();
            Price = price;
            Category = category.Trim();
            Stock = stock;
        }

        public void Update(string name, decimal price, string category, int stock)
        {
            EnsureIsValid(name, price, category, stock);
            Name = name.Trim();
            Price = price;
            Category = category.Trim();
            Stock = stock;
        }

        private static void EnsureIsValid(string name, decimal price, string category, int stock)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new DomainException("El nombre es requerido");
            if (price <= 0) throw new DomainException("El precio debe ser mayor que 0");
            if (string.IsNullOrWhiteSpace(category)) throw new DomainException("La categoría es requerida");
            if (stock < 0) throw new DomainException("El stock no puede ser negativo");
        }
    }
}
