using MediatR;
using Products.SharedKernel;

namespace Products.Application.Commands
{
    public class DeleteProductCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
}
