using System.Threading.Tasks;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao;
using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Interfaces
{
    public interface IUsuarioAutenticacaoReadProvider
    {
        Task<UsuarioAutenticacao> ObterPorEmail(string email);
    }
} 