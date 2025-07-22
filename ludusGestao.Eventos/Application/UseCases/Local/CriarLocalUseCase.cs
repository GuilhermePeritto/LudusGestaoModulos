using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.DTOs.Local;
using ludusGestao.Eventos.Application.Mappers.local;
using ludusGestao.Eventos.Application.Validations.Local;
using ludusGestao.Eventos.Domain.Providers;
using FluentValidation;
using ludusGestao.Eventos.Domain.Specifications;

namespace ludusGestao.Eventos.Application.UseCases.Local
{
    public class CriarLocalUseCase
    {
        private readonly ILocalWriteProvider _writeProvider;
        private readonly ILocalReadProvider _readProvider;
        private readonly CriarLocalValidation _validation;
        private readonly LocalMapeador _mapeador;

        public CriarLocalUseCase(ILocalWriteProvider writeProvider, ILocalReadProvider readProvider)
        {
            _writeProvider = writeProvider;
            _readProvider = readProvider;
            _validation = new CriarLocalValidation(_readProvider);
            _mapeador = new LocalMapeador();
        }

        public async Task<LocalDTO> Executar(CriarLocalDTO dto)
        {
            var resultado = _validation.Validate(dto);
            if (!resultado.IsValid)
                throw new ValidationException(resultado.Errors);

            var entidade = _mapeador.Mapear(dto);

            // Validação de capacidade suficiente
            var capacidadeSpec = new LocalCapacidadeSuficienteSpecification(dto.Capacidade);
            if (!capacidadeSpec.IsSatisfiedBy(entidade))
                throw new ValidationException(capacidadeSpec.ErrorMessage);

            // Validação de disponibilidade
            var disponivelSpec = new LocalDisponivelSpecification();
            if (!disponivelSpec.IsSatisfiedBy(entidade))
                throw new ValidationException(disponivelSpec.ErrorMessage);

            await _writeProvider.Adicionar(entidade);
            await _writeProvider.SalvarAlteracoes();
            return _mapeador.Mapear(entidade);
        }
    }
} 