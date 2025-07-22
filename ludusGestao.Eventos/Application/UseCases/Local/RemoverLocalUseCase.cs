using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.Providers;
using FluentValidation;
using ludusGestao.Eventos.Domain.Specifications;
using System;

namespace ludusGestao.Eventos.Application.UseCases.Local
{
    public class RemoverLocalUseCase
    {
        private readonly ILocalWriteProvider _writeProvider;
        private readonly ILocalReadProvider _readProvider;

        public RemoverLocalUseCase(ILocalWriteProvider writeProvider, ILocalReadProvider readProvider)
        {
            _writeProvider = writeProvider;
            _readProvider = readProvider;
        }

        public async Task<bool> Executar(string id)
        {
            var entidade = await _readProvider.BuscarPorId(Guid.Parse(id));
            if (entidade == null)
                throw new ValidationException("Local não encontrado.");

            var disponivelSpec = new LocalDisponivelSpecification();
            if (!disponivelSpec.IsSatisfiedBy(entidade))
                throw new ValidationException(disponivelSpec.ErrorMessage);

            await _writeProvider.Remover(entidade.Id);
            await _writeProvider.SalvarAlteracoes();
            return true;
        }
    }
} 