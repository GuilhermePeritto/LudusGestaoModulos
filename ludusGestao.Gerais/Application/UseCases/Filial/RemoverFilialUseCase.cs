using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Providers;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Filial
{
    public class RemoverFilialUseCase
    {
        private readonly IFilialReadProvider _readProvider;
        private readonly IFilialWriteProvider _writeProvider;
        public RemoverFilialUseCase(IFilialReadProvider readProvider, IFilialWriteProvider writeProvider)
        {
            _readProvider = readProvider;
            _writeProvider = writeProvider;
        }
        public async Task<bool> Executar(Guid id)
        {
            var entidade = await _readProvider.BuscarPorId(id);
            if (entidade == null)
                throw new FluentValidation.ValidationException("Filial não encontrada.");
            await _writeProvider.Remover(id);
            await _writeProvider.SalvarAlteracoes();
            return true;
        }
    }
} 