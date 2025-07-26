using FluentValidation;
using ludusGestao.Eventos.Domain.Local;
using ludusGestao.Eventos.Domain.Local.Specifications;

namespace ludusGestao.Eventos.Domain.Local.Validations
{
    public class AtualizarLocalValidation : AbstractValidator<Local>
    {
        public AtualizarLocalValidation()
        {
            RuleFor(l => l.Nome)
                .NotEmpty().WithMessage("O nome do local é obrigatório.");
            RuleFor(l => l.Capacidade)
                .GreaterThan(0).WithMessage("A capacidade deve ser maior que zero.");
            var spec = new LocalAtivoSpecification();
            RuleFor(l => l)
                .Must(spec.IsSatisfiedBy)
                .WithMessage(spec.MensagemErro);
        }
    }
} 