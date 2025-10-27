using AutoMapper;
using Moq;
using Products.Application.Handlers;
using Products.Application.Queries;
using Products.Application.DTOs;
using Products.Application.Abstractions.Interfaces;
using Products.Domain.Entities;

namespace Products.Application.Tests.Handlers
{
    [TestFixture]
    public class GetFilteredProductsHandlerTests
    {
        private Mock<IProductRepository> _repoMock;
        private Mock<IMapper> _mapperMock;
        private GetFilteredProductsHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetFilteredProductsHandler(_repoMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnFilteredProducts()
        {
            var filteredProducts = new List<Product>
            {
                new Product("Laptop", 1200, "Electronics", 3),
                new Product("Mouse", 25, "Electronics", 20)
            };

            _repoMock.Setup(r => r.GetFilteredAsync("Electronics", 20, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(filteredProducts);

            _mapperMock.Setup(m => m.Map<IEnumerable<ProductDto>>(filteredProducts))
                       .Returns(filteredProducts.Select(p => new ProductDto { Name = p.Name, Price = p.Price }));

            var query = new GetFilteredProductsQuery { Categoria = "Electronics", PrecioMin = 20 };
            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.AreEqual(2, result.Count());
        }
    }
}
