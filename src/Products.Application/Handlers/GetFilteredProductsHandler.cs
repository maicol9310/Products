using AutoMapper;
using MediatR;
using Products.Application.Abstractions.Interfaces;
using Products.Application.DTOs;
using Products.Application.Queries;

namespace Products.Application.Handlers
{
    public class GetFilteredProductsHandler : IRequestHandler<GetFilteredProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public GetFilteredProductsHandler(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetFilteredProductsQuery request, CancellationToken cancellationToken)
        {
            var filteredEntities = await _repo.GetFilteredAsync(request.Categoria, request.PrecioMin, cancellationToken);
            return _mapper.Map<IEnumerable<ProductDto>>(filteredEntities);
        }
    }
}
