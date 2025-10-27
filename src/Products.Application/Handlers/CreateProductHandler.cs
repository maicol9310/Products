using MediatR;
using Products.Application.Commands;
using Products.Application.Abstractions.Interfaces;
using Products.Domain.Entities;
using Products.SharedKernel;

namespace Products.Application.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, Result<int>>
    {
        private readonly IProductRepository _repo;

        public CreateProductHandler(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<int>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var product = new Product(request.Name, request.Price, request.Category, request.Stock);
                var id = await _repo.CreateAsync(product, cancellationToken);
                return Result<int>.Ok(id);
            }
            catch (DomainException dex)
            {
                return Result<int>.Fail(dex.Message);
            }
            catch (Exception ex)
            {
                return Result<int>.Fail("Error al crear producto: " + ex.Message);
            }
        }
    }
}
