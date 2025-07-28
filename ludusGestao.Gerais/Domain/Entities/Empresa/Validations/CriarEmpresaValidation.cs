using FluentValidation;
using ludusGestao.Gerais.Domain.Empresa;

namespace ludusGestao.Gerais.Domain.Empresa.Validations
{
    public class CriarEmpresaValidation : AbstractValidator<Empresa>
    {
        public CriarEmpresaValidation()
        {
            RuleFor(e => e.Nome)
                .NotEmpty().WithMessage("O nome da empresa é obrigatório.")
                .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");

            RuleFor(e => e.Cnpj)
                .NotNull().WithMessage("O CNPJ é obrigatório.");

            RuleFor(e => e.Email)
                .NotNull().WithMessage("O email é obrigatório.");

            RuleFor(e => e.Telefone)
                .NotNull().WithMessage("O telefone é obrigatório.");
        }
    }
} 