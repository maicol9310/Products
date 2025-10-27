using Moq;
using Products.Application.Commands;
using Products.Application.Handlers;
using Products.Application.Abstractions.Interfaces;
using Products.Domain.Entities;

namespace Products.Application.Tests.Handlers
{
    [TestFixture]
    public class UpdateProductHandlerTests
    {
        private Mock<IProductRepository> _repoMock;
        private UpdateProductHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IProductRepository>();
            _handler = new UpdateProductHandler(_repoMock.Object);
        }

        [Test]
        public async Task Handle_ProductExists_ShouldReturnSuccess()
        {
            var product = new Product("Laptop", 1000, "Electronics", 5);

            _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(product);

            _repoMock.Setup(r => r.UpdateAsync(product, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(true);

            var command = new UpdateProductCommand
            {
                Id = 1,
                Name = "Laptop Pro",
                Price = 1500,
                Category = "Electronics",
                Stock = 10
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        public async Task Handle_ProductNotFound_ShouldReturnFailure()
        {
            _repoMock.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                     .ReturnsAsync((Product?)null);

            var command = new UpdateProductCommand
            {
                Id = 999,
                Name = "Nonexistent",
                Price = 0,
                Category = "None",
                Stock = 0
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Producto no encontrado", result.Error);
        }
    }
}
