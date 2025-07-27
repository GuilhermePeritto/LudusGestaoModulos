using System;
using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.Entities.Local.Interfaces;
using LudusGestao.Shared.Notificacao;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Eventos.Domain.Entities.Local.UseCases
{
    public class BuscarLocalPorIdUseCase : IBuscarLocalPorIdUseCase
    {
        private readonly ILocalReadProvider _localReadProvider;
        private readonly INotificador _notificador;

        public BuscarLocalPorIdUseCase(ILocalReadProvider localReadProvider, INotificador notificador)
        {
            _localReadProvider = localReadProvider;
            _notificador = notificador;
        }

        public async Task<ludusGestao.Eventos.Domain.Entities.Local.Local> Executar(Guid id)
        {
            var queryParams = QueryParamsHelper.BuscarPorId(id);
            var local = await _localReadProvider.Buscar(queryParams);

            if (local == null)
            {
                _notificador.Handle(new LudusGestao.Shared.Notificacao.Notificacao("Local não encontrado"));
            }

            return local;
        }
    }
} 