using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.DTOs.Local;
using ludusGestao.Eventos.Application.Mappers.local;
using ludusGestao.Eventos.Domain.Providers;
using LudusGestao.Shared.Domain.Common;

namespace ludusGestao.Eventos.Application.UseCases.Local
{
    public class ListarLocaisUseCase
    {
        private readonly ILocalReadProvider _readProvider;
        private readonly LocalMapeador _mapeador;

        public ListarLocaisUseCase(ILocalReadProvider readProvider)
        {
            _readProvider = readProvider;
            _mapeador = new LocalMapeador();
        }

        public async Task<(IEnumerable<LocalDTO> Itens, int Total)> Executar(QueryParamsBase query)
        {
            var entidades = await _readProvider.BuscarPaginado(query.Page, query.Limit);
            var total = await _readProvider.Contar();
            var dtos = entidades.Select(e => _mapeador.Mapear(e));
            return (dtos, total);
        }
    }
} 