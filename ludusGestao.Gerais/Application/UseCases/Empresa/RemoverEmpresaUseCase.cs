using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Repositories;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Empresa
{
    public class RemoverEmpresaUseCase
    {
        private readonly IEmpresaRepository _repository;
        public RemoverEmpresaUseCase(IEmpresaRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> Executar(Guid id)
        {
            var entidade = await _repository.BuscarPorId(id);
            if (entidade == null)
                throw new ValidationException("Empresa não encontrada.");
            await _repository.Remover(entidade);
            return true;
        }
    }
} 