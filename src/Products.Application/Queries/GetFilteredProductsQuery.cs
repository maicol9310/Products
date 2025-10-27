using MediatR;
using Products.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Application.Queries
{
    public class GetFilteredProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
        public string Categoria { get; set; } = string.Empty;
        public decimal PrecioMin { get; set; } = 0;
    }
}
