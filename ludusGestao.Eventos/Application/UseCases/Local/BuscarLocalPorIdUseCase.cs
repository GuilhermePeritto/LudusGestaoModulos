using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.DTOs.Local;
using ludusGestao.Eventos.Application.Mappers.local;
using ludusGestao.Eventos.Domain.Repositories;
using FluentValidation;

namespace ludusGestao.Eventos.Application.UseCases.Local
{
    public class BuscarLocalPorIdUseCase
    {
        private readonly ILocalRepository _repository;
        private readonly LocalMapeador _mapeador;

        public BuscarLocalPorIdUseCase(ILocalRepository repository)
        {
            _repository = repository;
            _mapeador = new LocalMapeador();
        }

        public async Task<LocalDTO> Executar(string id)
        {
            var entidade = await _repository.BuscarPorId(id);
            if (entidade == null)
                throw new ValidationException("Local não encontrado.");

            return _mapeador.Mapear(entidade);
        }
    }
} 