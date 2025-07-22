using FluentValidation;
using ludusGestao.Autenticacao.Application.DTOs;

namespace ludusGestao.Autenticacao.Application.Validations
{
    public class AtualizarTokenValidation : AbstractValidator<AtualizarTokenDTO>
    {
        public AtualizarTokenValidation()
        {
            RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("Refresh token obrigatório.");
        }
    }
} 