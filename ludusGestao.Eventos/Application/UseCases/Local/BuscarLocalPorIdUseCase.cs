using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.DTOs.Local;
using ludusGestao.Eventos.Application.Mappers.local;
using ludusGestao.Eventos.Domain.Providers;
using FluentValidation;

namespace ludusGestao.Eventos.Application.UseCases.Local
{
    public class BuscarLocalPorIdUseCase
    {
        private readonly ILocalReadProvider _readProvider;
        private readonly LocalMapeador _mapeador;

        public BuscarLocalPorIdUseCase(ILocalReadProvider readProvider)
        {
            _readProvider = readProvider;
            _mapeador = new LocalMapeador();
        }

        public async Task<LocalDTO> Executar(string id)
        {
            var entidade = await _readProvider.BuscarPorId(Guid.Parse(id));
            if (entidade == null)
                throw new ValidationException("Local não encontrado.");

            return _mapeador.Mapear(entidade);
        }
    }
} 