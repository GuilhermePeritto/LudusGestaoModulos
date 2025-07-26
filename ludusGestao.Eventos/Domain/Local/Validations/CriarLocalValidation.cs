using FluentValidation;
using ludusGestao.Eventos.Domain.Local;
using ludusGestao.Eventos.Domain.Local.Specifications;

namespace ludusGestao.Eventos.Domain.Local.Validations
{
    public class CriarLocalValidation : AbstractValidator<Local>
    {
        public CriarLocalValidation()
        {
            RuleFor(l => l.Nome)
                .NotEmpty().WithMessage("O nome do local é obrigatório.");
            RuleFor(l => l.Capacidade)
                .GreaterThan(0).WithMessage("A capacidade deve ser maior que zero.");
            RuleFor(l => l)
                .Must(l => new LocalAtivoSpecification().IsSatisfiedBy(l))
                .WithMessage("O local precisa estar ativo para ser criado.");
            // Adicione outras specifications reutilizáveis aqui
        }
    }
} 