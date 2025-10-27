using MediatR;
using Products.Application.Commands;
using Products.Application.Abstractions.Interfaces;
using Products.SharedKernel;

namespace Products.Application.Handlers
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Result>
    {
        private readonly IProductRepository _repo;

        public DeleteProductHandler(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var exists = await _repo.GetByIdAsync(request.Id, cancellationToken);
                if (exists == null) return Result.Fail("Producto no encontrado");

                var ok = await _repo.DeleteAsync(request.Id, cancellationToken);
                return ok ? Result.Ok() : Result.Fail("No se pudo eliminar");
            }
            catch (Exception ex)
            {
                return Result.Fail("Error al eliminar: " + ex.Message);
            }
        }
    }
}
