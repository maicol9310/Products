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
    public class GetAllProductsHandlerTests
    {
        private Mock<IProductRepository> _repoMock;
        private Mock<IMapper> _mapperMock;
        private GetAllProductsHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllProductsHandler(_repoMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnMappedProducts()
        {
            var products = new List<Product>
            {
                new Product("Laptop", 1000, "Electronics", 5),
                new Product("Mouse", 20, "Electronics", 50)
            };

            _repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                     .ReturnsAsync(products);

            _mapperMock.Setup(m => m.Map<IEnumerable<ProductDto>>(products))
                       .Returns(new List<ProductDto>
                       {
                           new ProductDto { Name = "Laptop", Price = 1000 },
                           new ProductDto { Name = "Mouse", Price = 20 }
                       });

            var result = await _handler.Handle(new GetAllProductsQuery(), CancellationToken.None);

            Assert.AreEqual(2, result.Count());
        }
    }
}
