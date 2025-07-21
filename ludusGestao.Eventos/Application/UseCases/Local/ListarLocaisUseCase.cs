using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.DTOs.Local;
using ludusGestao.Eventos.Application.Mappers.local;
using ludusGestao.Eventos.Domain.Repositories;
using LudusGestao.Shared.Domain.Common;

namespace ludusGestao.Eventos.Application.UseCases.Local
{
    public class ListarLocaisUseCase
    {
        private readonly ILocalRepository _repository;
        private readonly LocalMapeador _mapeador;

        public ListarLocaisUseCase(ILocalRepository repository)
        {
            _repository = repository;
            _mapeador = new LocalMapeador();
        }

        public async Task<(IEnumerable<LocalDTO> Itens, int Total)> Executar(QueryParamsBase query)
        {
            var (entidades, total) = await _repository.ListarPaginado(query);
            var dtos = entidades.Select(e => _mapeador.Mapear(e));
            return (dtos, total);
        }
    }
} 