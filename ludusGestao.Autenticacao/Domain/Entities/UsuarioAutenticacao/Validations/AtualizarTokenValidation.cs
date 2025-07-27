using FluentValidation;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.DTOs;

namespace ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Validations
{
    public class AtualizarTokenValidation : AbstractValidator<AtualizarTokenDTO>
    {
        public AtualizarTokenValidation()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("O refresh token é obrigatório.");
        }
    }
} 