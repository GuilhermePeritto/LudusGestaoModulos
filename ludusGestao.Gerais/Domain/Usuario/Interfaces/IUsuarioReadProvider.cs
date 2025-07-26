using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Usuario;

namespace ludusGestao.Gerais.Domain.Usuario.Interfaces
{
    public interface IUsuarioReadProvider
    {
        Task<IEnumerable<Usuario>> Listar();
        Task<IEnumerable<Usuario>> Listar(QueryParamsBase queryParams);
        Task<Usuario> Buscar(QueryParamsBase queryParams);
    }
} 