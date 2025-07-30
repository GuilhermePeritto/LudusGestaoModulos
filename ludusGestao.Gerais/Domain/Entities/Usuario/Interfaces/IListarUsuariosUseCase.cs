using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.QueryParams;
using ludusGestao.Gerais.Domain.Usuario.DTOs;

namespace ludusGestao.Gerais.Domain.Usuario.Interfaces
{
    public interface IListarUsuariosUseCase
    {
        Task<IEnumerable<UsuarioDTO>> Executar(QueryParamsBase query);
    }
} 