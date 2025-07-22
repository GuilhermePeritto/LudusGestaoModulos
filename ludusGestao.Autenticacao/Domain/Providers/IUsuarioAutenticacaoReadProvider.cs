using System.Threading.Tasks;
using ludusGestao.Autenticacao.Domain.Entities;

namespace ludusGestao.Autenticacao.Domain.Providers
{
    public interface IUsuarioAutenticacaoReadProvider
    {
        Task<UsuarioAutenticacao> ObterPorLogin(string login);
    }
} 