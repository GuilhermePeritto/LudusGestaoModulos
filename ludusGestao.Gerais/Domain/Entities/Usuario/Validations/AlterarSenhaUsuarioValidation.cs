using FluentValidation;

namespace ludusGestao.Gerais.Domain.Usuario.Validations
{
    public class AlterarSenhaUsuarioValidation : AbstractValidator<string>
    {
        public AlterarSenhaUsuarioValidation()
        {
            RuleFor(senha => senha)
                .NotEmpty().WithMessage("A nova senha é obrigatória.")
                .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.")
                .MaximumLength(100).WithMessage("A senha deve ter no máximo 100 caracteres.");
        }
    }
} 