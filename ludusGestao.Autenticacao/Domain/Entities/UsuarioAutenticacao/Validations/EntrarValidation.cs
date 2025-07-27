using FluentValidation;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.DTOs;

namespace ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Validations
{
    public class EntrarValidation : AbstractValidator<EntrarDTO>
    {
        public EntrarValidation()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O email é obrigatório.")
                .EmailAddress().WithMessage("O email deve ter um formato válido.");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.");
        }
    }
} 