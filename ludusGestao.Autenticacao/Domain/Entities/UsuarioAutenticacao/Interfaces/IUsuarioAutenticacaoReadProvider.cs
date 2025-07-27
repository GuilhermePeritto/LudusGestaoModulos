using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Interfaces
{
    public interface IUsuarioAutenticacaoReadProvider : IReadProvider<UsuarioAutenticacao>
    {
        Task<UsuarioAutenticacao> ObterPorEmail(string email);
    }
} 