using AutoMapper;
using MediatR;
using Products.Application.DTOs;
using Products.Application.Abstractions.Interfaces;
using Products.Application.Queries;

namespace Products.Application.Handlers
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly IProductRepository _repo;
        private readonly IMapper _mapper;

        public GetProductByIdHandler(IProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(request.Id, cancellationToken);
            return _mapper.Map<ProductDto?>(entity);
        }
    }
}
