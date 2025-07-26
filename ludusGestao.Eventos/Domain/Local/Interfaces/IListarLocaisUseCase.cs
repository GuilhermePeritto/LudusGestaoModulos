using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Eventos.Domain.Local;

namespace ludusGestao.Eventos.Domain.Local.Interfaces
{
    public interface IListarLocaisUseCase
    {
        Task<IEnumerable<Local>> Executar(QueryParamsBase queryParams);
    }
} 