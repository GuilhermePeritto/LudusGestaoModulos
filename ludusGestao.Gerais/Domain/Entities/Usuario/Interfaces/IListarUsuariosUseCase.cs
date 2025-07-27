using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Usuario;

namespace ludusGestao.Gerais.Domain.Usuario.Interfaces
{
    public interface IListarUsuariosUseCase
    {
        Task<IEnumerable<Usuario>> Executar(QueryParamsBase query);
    }
} 