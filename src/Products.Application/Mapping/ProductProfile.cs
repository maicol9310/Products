using AutoMapper;
using Products.Application.DTOs;
using Products.Domain.Entities;

namespace Products.Application.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
