using FluentValidation;
using Products.Application.Commands;

namespace Products.Application.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Nombre requerido");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Precio debe ser mayor que 0");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Categoria requerida");
            RuleFor(x => x.Stock).GreaterThanOrEqualTo(0).WithMessage("Stock no puede ser negativo");
        }
    }
}
