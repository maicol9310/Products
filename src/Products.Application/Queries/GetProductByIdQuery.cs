using MediatR;
using Products.Application.DTOs;

namespace Products.Application.Queries
{
    public class GetProductByIdQuery : IRequest<ProductDto?>
    {
        public int Id { get; set; }
    }
}
