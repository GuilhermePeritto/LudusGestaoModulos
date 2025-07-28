using FluentValidation;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.DTOs;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Specifications;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Interfaces;
using LudusGestao.Shared.Security;

namespace ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Validations
{
    public class EntrarValidation : AbstractValidator<EntrarDTO>
    {
        private readonly IUsuarioAutenticacaoReadProvider _usuarioProvider;
        private readonly IPasswordHelper _passwordHelper;

        public EntrarValidation(IUsuarioAutenticacaoReadProvider usuarioProvider, IPasswordHelper passwordHelper)
        {
            _usuarioProvider = usuarioProvider;
            _passwordHelper = passwordHelper;

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O email é obrigatório.")
                .EmailAddress().WithMessage("O email deve ter um formato válido.");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.");

            RuleFor(x => x)
                .MustAsync(async (dto, cancellation) =>
                {
                    var usuario = await _usuarioProvider.ObterPorEmail(dto.Email);
                    return usuario != null;
                })
                .WithMessage("Usuário não encontrado.");

            RuleFor(x => x)
                .MustAsync(async (dto, cancellation) =>
                {
                    var usuario = await _usuarioProvider.ObterPorEmail(dto.Email);
                    if (usuario == null) return false;
                    
                    return new UsuarioAtivoSpecification().IsSatisfiedBy(usuario);
                })
                .WithMessage("Usuário inativo.");

            RuleFor(x => x)
                .MustAsync(async (dto, cancellation) =>
                {
                    var usuario = await _usuarioProvider.ObterPorEmail(dto.Email);
                    if (usuario == null) return false;
                    
                    return _passwordHelper.VerificarSenha(dto.Senha, usuario.Senha);
                })
                .WithMessage("Senha inválida.");
        }
    }
} 