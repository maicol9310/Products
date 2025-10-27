using MediatR;
using Products.SharedKernel;

namespace Products.Application.Commands
{
    public class CreateProductCommand : IRequest<Result<int>>
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}
