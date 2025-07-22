using FluentValidation;
using ludusGestao.Autenticacao.Application.DTOs;

namespace ludusGestao.Autenticacao.Application.Validations
{
    public class EntrarValidation : AbstractValidator<EntrarDTO>
    {
        public EntrarValidation()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email obrigatório.");
            RuleFor(x => x.Senha).NotEmpty().WithMessage("Senha obrigatória.");
        }
    }
} 