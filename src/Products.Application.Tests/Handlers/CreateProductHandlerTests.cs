using Moq;
using Products.Application.Commands;
using Products.Application.Handlers;
using Products.Application.Abstractions.Interfaces;
using Products.Domain.Entities;
using Products.SharedKernel;

namespace Products.Application.Tests.Handlers
{
    [TestFixture]
    public class CreateProductHandlerTests
    {
        private Mock<IProductRepository> _repoMock;
        private CreateProductHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IProductRepository>();
            _handler = new CreateProductHandler(_repoMock.Object);
        }

        [Test]
        public async Task Handle_ValidRequest_ShouldReturnSuccessWithId()
        {
            _repoMock.Setup(r => r.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(1);

            var command = new CreateProductCommand
            {
                Name = "Laptop",
                Price = 1200,
                Category = "Electronics",
                Stock = 5
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(1, result.Value);
        }

        [Test]
        public async Task Handle_DomainException_ShouldReturnFailure()
        {
            _repoMock.Setup(r => r.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                     .ThrowsAsync(new DomainException("Error de dominio"));

            var command = new CreateProductCommand
            {
                Name = "Laptop",
                Price = 1200,
                Category = "Electronics",
                Stock = 5
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Error de dominio", result.Error);
        }

        [Test]
        public async Task Handle_GenericException_ShouldReturnFailure()
        {
            _repoMock.Setup(r => r.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                     .ThrowsAsync(new System.Exception("Error inesperado"));

            var command = new CreateProductCommand
            {
                Name = "Laptop",
                Price = 1200,
                Category = "Electronics",
                Stock = 5
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.Error.Contains("Error al crear producto"));
        }
    }
}
