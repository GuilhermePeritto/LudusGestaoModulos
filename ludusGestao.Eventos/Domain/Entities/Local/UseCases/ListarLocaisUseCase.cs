using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.Entities.Local.Interfaces;
using LudusGestao.Shared.Notificacao;

namespace ludusGestao.Eventos.Domain.Entities.Local.UseCases
{
    public class ListarLocaisUseCase : IListarLocaisUseCase
    {
        private readonly ILocalReadProvider _localReadProvider;
        private readonly INotificador _notificador;

        public ListarLocaisUseCase(ILocalReadProvider localReadProvider, INotificador notificador)
        {
            _localReadProvider = localReadProvider;
            _notificador = notificador;
        }

        public async Task<IEnumerable<ludusGestao.Eventos.Domain.Entities.Local.Local>> Executar()
        {
            var locais = await _localReadProvider.Listar();
            return locais;
        }
    }
} 