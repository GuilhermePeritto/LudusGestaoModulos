using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Providers;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Empresa
{
    public class RemoverEmpresaUseCase
    {
        private readonly IEmpresaReadProvider _readProvider;
        private readonly IEmpresaWriteProvider _writeProvider;
        public RemoverEmpresaUseCase(IEmpresaReadProvider readProvider, IEmpresaWriteProvider writeProvider)
        {
            _readProvider = readProvider;
            _writeProvider = writeProvider;
        }
        public async Task<bool> Executar(Guid id)
        {
            var entidade = await _readProvider.BuscarPorId(id);
            if (entidade == null)
                throw new FluentValidation.ValidationException("Empresa não encontrada.");
            await _writeProvider.Remover(id);
            await _writeProvider.SalvarAlteracoes();
            return true;
        }
    }
} 