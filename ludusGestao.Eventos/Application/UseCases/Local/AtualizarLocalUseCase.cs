using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.DTOs.Local;
using ludusGestao.Eventos.Application.Mappers.local;
using ludusGestao.Eventos.Domain.Repositories;
using FluentValidation;
using ludusGestao.Eventos.Application.Validations.Local;

namespace ludusGestao.Eventos.Application.UseCases.Local
{
    public class AtualizarLocalUseCase
    {
        private readonly ILocalRepository _repository;
        private readonly LocalMapeador _mapeador;

        public AtualizarLocalUseCase(ILocalRepository repository)
        {
            _repository = repository;
            _mapeador = new LocalMapeador();
        }

        public async Task<LocalDTO> Executar(string id, AtualizarLocalDTO dto)
        {
            var entidade = await _repository.BuscarPorId(id);
            if (entidade == null)
                throw new ValidationException("Local não encontrado.");

            var validation = new AtualizarLocalValidation(_repository, id);
            var resultado = await validation.ValidateAsync(dto);
            if (!resultado.IsValid)
                throw new ValidationException(resultado.Errors);

            _mapeador.Mapear(dto, entidade);
            await _repository.Atualizar(entidade);
            return _mapeador.Mapear(entidade);
        }
    }
} 