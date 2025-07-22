using FluentValidation;
using ludusGestao.Eventos.Domain.DTOs.Local;
using ludusGestao.Eventos.Domain.Providers;
using ludusGestao.Eventos.Domain.Specifications;

namespace ludusGestao.Eventos.Application.Validations.Local
{
    public class AtualizarLocalValidation : AbstractValidator<ludusGestao.Eventos.Domain.DTOs.Local.AtualizarLocalDTO>
    {
        public AtualizarLocalValidation(ILocalReadProvider readProvider, string idAtual)
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MinimumLength(3)
                .MustAsync(async (dto, nome, cancellation) =>
                {
                    var localExistente = await readProvider.BuscarPorId(Guid.Parse(idAtual));
                    if (localExistente != null && localExistente.Nome == nome)
                        return true; // Não mudou o nome, não precisa validar unicidade
                    return new LocalNomeUnicoSpecification(readProvider).IsSatisfiedBy(nome);
                })
                .WithMessage(new LocalNomeUnicoSpecification(readProvider).ErrorMessage);

            RuleFor(x => x.Rua).NotEmpty().WithMessage("Rua é obrigatória.");
            RuleFor(x => x.Numero).NotEmpty().WithMessage("Número é obrigatório.");
            RuleFor(x => x.Bairro).NotEmpty().WithMessage("Bairro é obrigatório.");
            RuleFor(x => x.Cidade).NotEmpty().WithMessage("Cidade é obrigatória.");
            RuleFor(x => x.Estado).NotEmpty().WithMessage("Estado é obrigatória.");
            RuleFor(x => x.Cep).NotEmpty().WithMessage("CEP é obrigatório.");
            RuleFor(x => x.Capacidade).GreaterThan(0).WithMessage("Capacidade deve ser maior que zero.");
        }
    }
} 