using FluentValidation;
using ludusGestao.Eventos.Domain.Specifications;
using ludusGestao.Eventos.Domain.DTOs.Local;
using ludusGestao.Eventos.Domain.Providers;

namespace ludusGestao.Eventos.Application.Validations.Local
{
    public class CriarLocalValidation : AbstractValidator<ludusGestao.Eventos.Domain.DTOs.Local.CriarLocalDTO>
    {
        public CriarLocalValidation(ILocalReadProvider readProvider)
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MinimumLength(3)
                .Must(nome => new LocalNomeUnicoSpecification(readProvider).IsSatisfiedBy(nome))
                .WithMessage(new LocalNomeUnicoSpecification(readProvider).ErrorMessage);

            RuleFor(x => x.Rua).NotEmpty().WithMessage("Rua é obrigatória.");
            RuleFor(x => x.Numero).NotEmpty().WithMessage("Número é obrigatório.");
            RuleFor(x => x.Bairro).NotEmpty().WithMessage("Bairro é obrigatório.");
            RuleFor(x => x.Cidade).NotEmpty().WithMessage("Cidade é obrigatória.");
            RuleFor(x => x.Estado).NotEmpty().WithMessage("Estado é obrigatório.");
            RuleFor(x => x.Cep).NotEmpty().WithMessage("CEP é obrigatório.");
            RuleFor(x => x.Capacidade).GreaterThan(0).WithMessage("Capacidade deve ser maior que zero.");
        }
    }
} 