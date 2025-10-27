using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Application.Commands;
using Products.Application.Queries;

namespace Products.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Policy = "RequireAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllProductsQuery());
            return Ok(result);
        }

        [Authorize(Policy = "RequireAdmin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dto = await _mediator.Send(new GetProductByIdQuery { Id = id });
            if (dto == null) return NotFound(new { error = "Producto no encontrado" });
            return Ok(dto);
        }

        [Authorize(Policy = "RequireAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
        {
            var res = await _mediator.Send(command);
            if (res.IsFailure) return BadRequest(new { error = res.Error });
            var id = res.Value!;
            var dto = await _mediator.Send(new GetProductByIdQuery { Id = id });
            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }

        [Authorize(Policy = "RequireAdmin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductCommand command)
        {
            if (id != command.Id) return BadRequest(new { error = "Id no coincide" });
            var res = await _mediator.Send(command);
            if (res.IsFailure) return BadRequest(new { error = res.Error });
            return NoContent();
        }

        [Authorize(Policy = "RequireAdmin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _mediator.Send(new DeleteProductCommand { Id = id });
            if (res.IsFailure) return BadRequest(new { error = res.Error });
            return NoContent();
        }

        [Authorize(Policy = "RequireAdmin")]
        [HttpGet("filter")]
        public async Task<IActionResult> GetFiltered([FromQuery] string categoria, [FromQuery] decimal preciomin)
        {
            var query = new GetFilteredProductsQuery
            {
                Categoria = categoria,
                PrecioMin = preciomin
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
