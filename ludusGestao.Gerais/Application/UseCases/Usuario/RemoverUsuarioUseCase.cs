using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Providers;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Usuario
{
    public class RemoverUsuarioUseCase
    {
        private readonly IUsuarioReadProvider _readProvider;
        private readonly IUsuarioWriteProvider _writeProvider;
        public RemoverUsuarioUseCase(IUsuarioReadProvider readProvider, IUsuarioWriteProvider writeProvider)
        {
            _readProvider = readProvider;
            _writeProvider = writeProvider;
        }
        public async Task<bool> Executar(Guid id)
        {
            var entidade = await _readProvider.BuscarPorId(id);
            if (entidade == null)
                throw new FluentValidation.ValidationException("Usuário não encontrado.");
            await _writeProvider.Remover(id);
            await _writeProvider.SalvarAlteracoes();
            return true;
        }
    }
} 