using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Eventos.Domain.Entities.Local;
using ludusGestao.Eventos.Domain.Entities.Local.Interfaces;

namespace ludusGestao.Eventos.Domain.Entities.Local.UseCases
{
    public class ListarLocaisUseCase : BaseUseCase, IListarLocaisUseCase
    {
        private readonly ILocalReadProvider _localReadProvider;

        public ListarLocaisUseCase(ILocalReadProvider localReadProvider, INotificador notificador)
            : base(notificador)
        {
            _localReadProvider = localReadProvider;
        }

        public async Task<IEnumerable<Local>> Executar()
        {
            var locais = await _localReadProvider.Listar();
            return locais;
        }
    }
} 