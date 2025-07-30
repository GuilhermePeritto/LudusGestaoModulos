using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao;
using LudusGestao.Shared.Domain.QueryParams;

namespace ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Interfaces
{
    public interface IUsuarioAutenticacaoReadProvider
    {
        Task<UsuarioAutenticacao?> Buscar(QueryParamsBase queryParams);
    }
} 