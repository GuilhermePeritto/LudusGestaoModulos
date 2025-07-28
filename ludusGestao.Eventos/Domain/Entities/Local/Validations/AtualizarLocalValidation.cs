using FluentValidation;
using ludusGestao.Eventos.Domain.Entities.Local;

namespace ludusGestao.Eventos.Domain.Entities.Local.Validations
{
    public class AtualizarLocalValidation : AbstractValidator<Local>
    {
        public AtualizarLocalValidation()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");

            RuleFor(x => x.Descricao)
                .MaximumLength(500).WithMessage("Descrição deve ter no máximo 500 caracteres");

            RuleFor(x => x.Endereco)
                .NotNull().WithMessage("Endereço é obrigatório");

            RuleFor(x => x.Telefone)
                .NotNull().WithMessage("Telefone é obrigatório");
        }
    }
} 