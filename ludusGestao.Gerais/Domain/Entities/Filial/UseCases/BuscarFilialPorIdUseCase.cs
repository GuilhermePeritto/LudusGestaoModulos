using System;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.QueryParams.Helpers;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Gerais.Domain.Filial;
using ludusGestao.Gerais.Domain.Filial.Interfaces;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Gerais.Domain.Filial.UseCases
{
    public class BuscarFilialPorIdUseCase : BaseUseCase, IBuscarFilialPorIdUseCase
    {
        private readonly IFilialReadProvider _provider;

        public BuscarFilialPorIdUseCase(IFilialReadProvider provider, INotificador notificador)
            : base(notificador)
        {
            _provider = provider;
        }

        public async Task<Filial> Executar(Guid id)
        {
            var filial = await _provider.Buscar(QueryParamsHelper.FiltrarPorId(id));

            if (filial == null)
            {
                Notificar("Filial não encontrada");
                return null;
            }

            return filial;
        }
    }
}
