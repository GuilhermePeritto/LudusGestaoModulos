using System.Threading.Tasks;
using ludusGestao.Autenticacao.Domain.Entities;
using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Autenticacao.Domain.Providers
{
    public interface IUsuarioAutenticacaoReadProvider
    {
        Task<UsuarioAutenticacao> ObterPorEmail(Email email);
    }
} 