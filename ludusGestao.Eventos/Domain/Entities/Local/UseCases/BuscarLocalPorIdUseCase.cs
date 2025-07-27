using System;
using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.Entities.Local.Interfaces;
using LudusGestao.Shared.Notificacao;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Eventos.Domain.Entities.Local.UseCases
{
    public class BuscarLocalPorIdUseCase : BaseUseCase, IBuscarLocalPorIdUseCase
    {
        private readonly ILocalReadProvider _provider;

        public BuscarLocalPorIdUseCase(ILocalReadProvider provider, INotificador notificador)
            : base(notificador)
        {
            _provider = provider;
        }

        public async Task<Local> Executar(Guid id)
        {
            var local = await _provider.Buscar(QueryParamsHelper.FiltrarPorId(id));

            if (local == null)
            {
                Notificar("Local não encontrado");
                return null;
            }

            return local;
        }
    }
}