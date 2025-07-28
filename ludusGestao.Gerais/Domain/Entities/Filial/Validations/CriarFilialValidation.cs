using FluentValidation;
using ludusGestao.Gerais.Domain.Filial;

namespace ludusGestao.Gerais.Domain.Filial.Validations
{
    public class CriarFilialValidation : AbstractValidator<Filial>
    {
        public CriarFilialValidation()
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

            RuleFor(f => f.EmpresaId)
                .NotEmpty().WithMessage("O ID da empresa é obrigatório.");
        }
    }
} 