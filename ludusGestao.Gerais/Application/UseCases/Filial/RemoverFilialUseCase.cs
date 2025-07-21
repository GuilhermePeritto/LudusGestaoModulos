using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Repositories;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Filial
{
    public class RemoverFilialUseCase
    {
        private readonly IFilialRepository _repository;
        public RemoverFilialUseCase(IFilialRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> Executar(Guid id)
        {
            var entidade = await _repository.BuscarPorId(id);
            if (entidade == null)
                throw new ValidationException("Filial não encontrada.");
            await _repository.Remover(entidade);
            return true;
        }
    }
} 