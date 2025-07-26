using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Eventos.Domain.Local;

namespace ludusGestao.Eventos.Domain.Local.Interfaces
{
    public interface ILocalReadProvider
    {
        Task<IEnumerable<Local>> Listar();
        Task<IEnumerable<Local>> Listar(QueryParamsBase queryParams);
        Task<Local> Buscar(QueryParamsBase queryParams);
    }
}
