using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Filial;
using ludusGestao.Gerais.Domain.Filial.Interfaces;

namespace ludusGestao.Gerais.Domain.Filial.UseCases
{
    public class ListarFiliaisUseCase : BaseUseCase, IListarFiliaisUseCase
    {
        private readonly IFilialReadProvider _provider;

        public ListarFiliaisUseCase(IFilialReadProvider provider, INotificador notificador)
            : base(notificador)
        {
            _provider = provider;
        }

        public async Task<IEnumerable<Filial>> Executar(QueryParamsBase query)
        {
            return await _provider.Listar(query);
        }
    }
} 