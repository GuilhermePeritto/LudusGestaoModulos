using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.DTOs.Local;
using ludusGestao.Eventos.Application.Mappers.local;
using ludusGestao.Eventos.Domain.Repositories;

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

        public async Task<IEnumerable<LocalDTO>> Executar()
        {
            var entidades = await _repository.ListarTodos();
            return entidades.Select(e => _mapeador.Mapear(e));
        }
    }
} 