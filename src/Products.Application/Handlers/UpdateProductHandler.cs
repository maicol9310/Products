using MediatR;
using Products.Application.Commands;
using Products.Application.Abstractions.Interfaces;
using Products.SharedKernel;

namespace Products.Application.Handlers
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Result>
    {
        private readonly IProductRepository _repo;

        public UpdateProductHandler(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existing = await _repo.GetByIdAsync(request.Id, cancellationToken);
                if (existing == null) return Result.Fail("Producto no encontrado");

                existing.Update(request.Name, request.Price, request.Category, request.Stock);

                var ok = await _repo.UpdateAsync(existing, cancellationToken);
                return ok ? Result.Ok() : Result.Fail("No se pudo actualizar");
            }
            catch (DomainException dex)
            {
                return Result.Fail(dex.Message);
            }
            catch (Exception ex)
            {
                return Result.Fail("Error al actualizar: " + ex.Message);
            }
        }
    }
}
