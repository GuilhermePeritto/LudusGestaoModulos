using FluentValidation;
using ludusGestao.Gerais.Domain.Empresa;
using ludusGestao.Gerais.Domain.Empresa.Specifications;

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
                .NotEmpty().WithMessage("O email é obrigatório.")
                .EmailAddress().WithMessage("O email deve ter um formato válido.");

            RuleFor(e => e.Telefone)
                .NotEmpty().WithMessage("O telefone é obrigatório.");

            RuleFor(e => e)
                .Must(e => new EmpresaAtivaSpecification().IsSatisfiedBy(e))
                .WithMessage("A empresa precisa estar ativa para ser criada.");
        }
    }
} 