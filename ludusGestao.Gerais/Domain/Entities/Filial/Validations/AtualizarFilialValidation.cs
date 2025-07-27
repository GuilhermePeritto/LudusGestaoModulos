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

            RuleFor(f => f.Telefone)
                .NotNull().WithMessage("O telefone é obrigatório.");

            RuleFor(f => f.Email)
                .NotNull().WithMessage("O email é obrigatório.");

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