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
    public class GetProductByIdHandlerTests
    {
        private Mock<IProductRepository> _repoMock;
        private Mock<IMapper> _mapperMock;
        private GetProductByIdHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetProductByIdHandler(_repoMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task Handle_ProductExists_ShouldReturnMappedProduct()
        {
            var product = new Product("Laptop", 1000, "Electronics", 5);

            _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(product);

            _mapperMock.Setup(m => m.Map<ProductDto>(product))
                       .Returns(new ProductDto { Name = "Laptop", Price = 1000 });

            var result = await _handler.Handle(new GetProductByIdQuery { Id = 1 }, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual("Laptop", result!.Name);
        }

        [Test]
        public async Task Handle_ProductNotFound_ShouldReturnNull()
        {
            _repoMock.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                     .ReturnsAsync((Product?)null);

            var result = await _handler.Handle(new GetProductByIdQuery { Id = 999 }, CancellationToken.None);

            Assert.IsNull(result);
        }
    }
}
