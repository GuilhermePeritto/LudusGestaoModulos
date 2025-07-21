using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Repositories;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Usuario
{
    public class RemoverUsuarioUseCase
    {
        private readonly IUsuarioRepository _repository;
        public RemoverUsuarioUseCase(IUsuarioRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> Executar(Guid id)
        {
            var entidade = await _repository.BuscarPorId(id);
            if (entidade == null)
                throw new ValidationException("Usuário não encontrado.");
            await _repository.Remover(entidade);
            return true;
        }
    }
} 