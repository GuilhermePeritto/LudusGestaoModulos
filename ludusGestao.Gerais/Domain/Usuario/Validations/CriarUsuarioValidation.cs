using FluentValidation;
using ludusGestao.Gerais.Domain.Usuario;
using ludusGestao.Gerais.Domain.Usuario.Specifications;

namespace ludusGestao.Gerais.Domain.Usuario.Validations
{
    public class CriarUsuarioValidation : AbstractValidator<Usuario>
    {
        public CriarUsuarioValidation()
        {
            RuleFor(u => u.Nome)
                .NotEmpty().WithMessage("O nome do usuário é obrigatório.")
                .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");

            RuleFor(u => u.Email)
                .NotNull().WithMessage("O email é obrigatório.");

            RuleFor(u => u.Telefone)
                .NotEmpty().WithMessage("O telefone é obrigatório.");

            RuleFor(u => u.Cargo)
                .NotEmpty().WithMessage("O cargo é obrigatório.");

            RuleFor(u => u.EmpresaId)
                .NotEmpty().WithMessage("O ID da empresa é obrigatório.");

            RuleFor(u => u.Senha)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.");

            RuleFor(u => u)
                .Must(u => new UsuarioAtivoSpecification().IsSatisfiedBy(u))
                .WithMessage("O usuário precisa estar ativo para ser criado.");
        }
    }
} 