using FluentValidation;
using ludusGestao.Gerais.Domain.Usuario;
using ludusGestao.Gerais.Domain.Usuario.Specifications;

namespace ludusGestao.Gerais.Domain.Usuario.Validations
{
    public class AtualizarUsuarioValidation : AbstractValidator<Usuario>
    {
        public AtualizarUsuarioValidation()
        {
            RuleFor(u => u.Nome)
                .NotEmpty().WithMessage("O nome do usuário é obrigatório.")
                .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");

            RuleFor(u => u.Email)
                .NotNull().WithMessage("O email é obrigatório.");

            RuleFor(u => u.Telefone)
                .NotNull().WithMessage("O telefone é obrigatório.");

            RuleFor(u => u.Cargo)
                .NotEmpty().WithMessage("O cargo é obrigatório.");

            RuleFor(u => u)
                .Must(u => new UsuarioAtivoSpecification().IsSatisfiedBy(u))
                .WithMessage("O usuário precisa estar ativo para ser atualizado.");
        }
    }
} 