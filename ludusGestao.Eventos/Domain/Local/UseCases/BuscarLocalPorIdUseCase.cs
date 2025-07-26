using System;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Domain.Providers;
using ludusGestao.Eventos.Domain.Local;
using ludusGestao.Eventos.Domain.Local.Interfaces;

namespace ludusGestao.Eventos.Domain.Local.UseCases
{
    public class BuscarLocalPorIdUseCase : BaseUseCase
    {
        private readonly ILocalReadProvider _readProvider;
        public BuscarLocalPorIdUseCase(ILocalReadProvider readProvider, INotificador notificador)
            : base(notificador)
        {
            _readProvider = readProvider;
        }

        public async Task<Local> Executar(Guid id)
        {
            return await _readProvider.Buscar(QueryParamsHelper.BuscarPorId(id));
        }
    }
} 