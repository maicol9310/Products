using MediatR;
using Products.Application.DTOs;

namespace Products.Application.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductDto>> { }
}
