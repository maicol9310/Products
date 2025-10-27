using AutoMapper;
using MediatR;
using Products.Application.DTOs;
using Products.Application.Abstractions.Interfaces;
using Products.Application.Queries;

namespace Products.Application.Handlers
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public GetAllProductsHandler(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _repo.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ProductDto>>(entities);
        }
    }
}
