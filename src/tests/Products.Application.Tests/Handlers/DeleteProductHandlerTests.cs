using Moq;
using Products.Application.Commands;
using Products.Application.Handlers;
using Products.Application.Abstractions.Interfaces;
using Products.Domain.Entities;

namespace Products.Application.Tests.Handlers
{
    [TestFixture]
    public class DeleteProductHandlerTests
    {
        private Mock<IProductRepository> _repoMock;
        private DeleteProductHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IProductRepository>();
            _handler = new DeleteProductHandler(_repoMock.Object);
        }

        [Test]
        public async Task Handle_ProductExists_ShouldReturnSuccess()
        {
            var product = new Product("Laptop", 1200, "Electronics", 5);

            _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(product);

            _repoMock.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(true);

            var command = new DeleteProductCommand { Id = 1 };
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        public async Task Handle_ProductNotFound_ShouldReturnFailure()
        {
            _repoMock.Setup(r => r.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                     .ReturnsAsync((Product?)null);

            var command = new DeleteProductCommand { Id = 999 };
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Producto no encontrado", result.Error);
        }

        [Test]
        public async Task Handle_DeleteFails_ShouldReturnFailure()
        {
            var product = new Product("Laptop", 1200, "Electronics", 5);

            _repoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(product);

            _repoMock.Setup(r => r.DeleteAsync(1, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(false);

            var command = new DeleteProductCommand { Id = 1 };
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("No se pudo eliminar", result.Error);
        }

        [Test]
        public async Task Handle_Exception_ShouldReturnFailure()
        {
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                     .ThrowsAsync(new System.Exception("Error inesperado"));

            var command = new DeleteProductCommand { Id = 1 };
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Error.Contains("Error al eliminar"));
        }
    }
}