using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.DTOs.Local;
using ludusGestao.Eventos.Application.Mappers.local;
using ludusGestao.Eventos.Domain.Providers;
using FluentValidation;
using ludusGestao.Eventos.Application.Validations.Local;
using System;

namespace ludusGestao.Eventos.Application.UseCases.Local
{
    public class AtualizarLocalUseCase
    {
        private readonly ILocalWriteProvider _writeProvider;
        private readonly ILocalReadProvider _readProvider;
        private readonly LocalMapeador _mapeador;

        public AtualizarLocalUseCase(ILocalWriteProvider writeProvider, ILocalReadProvider readProvider)
        {
            _writeProvider = writeProvider;
            _readProvider = readProvider;
            _mapeador = new LocalMapeador();
        }

        public async Task<LocalDTO> Executar(string id, AtualizarLocalDTO dto)
        {
            var entidade = await _readProvider.BuscarPorId(Guid.Parse(id));
            if (entidade == null)
                throw new ValidationException("Local não encontrado.");

            var validation = new AtualizarLocalValidation(_readProvider, id);
            var resultado = await validation.ValidateAsync(dto);
            if (!resultado.IsValid)
                throw new ValidationException(resultado.Errors);

            _mapeador.Mapear(dto, entidade);
            await _writeProvider.Atualizar(entidade);
            await _writeProvider.SalvarAlteracoes();
            return _mapeador.Mapear(entidade);
        }
    }
} 