using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Eventos.Domain.Local;
using ludusGestao.Eventos.Domain.Local.Interfaces;

namespace ludusGestao.Eventos.Domain.Local.UseCases
{
    public class ListarLocaisUseCase : BaseUseCase
    {
        private readonly ILocalReadProvider _readProvider;
        public ListarLocaisUseCase(ILocalReadProvider readProvider, INotificador notificador)
            : base(notificador)
        {
            _readProvider = readProvider;
        }

        public async Task<IEnumerable<Local>> Executar(QueryParamsBase queryParams)
        {
            return await _readProvider.Listar(queryParams);
        }
    }
} 