using FluentValidation;
using ludusGestao.Gerais.Domain.Filial;
using ludusGestao.Gerais.Domain.Filial.Specifications;

namespace ludusGestao.Gerais.Domain.Filial.Validations
{
    public class AtualizarFilialValidation : AbstractValidator<Filial>
    {
        public AtualizarFilialValidation()
        {
            RuleFor(f => f.Nome)
                .NotEmpty().WithMessage("O nome da filial é obrigatório.")
                .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");

            RuleFor(f => f.Codigo)
                .NotEmpty().WithMessage("O código da filial é obrigatório.")
                .MaximumLength(50).WithMessage("O código deve ter no máximo 50 caracteres.");

            RuleFor(f => f.Telefone)
                .NotEmpty().WithMessage("O telefone é obrigatório.");

            RuleFor(f => f.Email)
                .NotEmpty().WithMessage("O email é obrigatório.");

            RuleFor(f => f.Cnpj)
                .NotNull().WithMessage("O CNPJ é obrigatório.");

            RuleFor(f => f.Responsavel)
                .NotEmpty().WithMessage("O responsável é obrigatório.");

            RuleFor(f => f)
                .Must(f => new FilialAtivaSpecification().IsSatisfiedBy(f))
                .WithMessage("A filial precisa estar ativa para ser atualizada.");
        }
    }
} 