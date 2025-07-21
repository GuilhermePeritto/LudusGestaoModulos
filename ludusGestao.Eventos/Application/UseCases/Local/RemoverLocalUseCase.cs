using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.Repositories;
using FluentValidation;
using ludusGestao.Eventos.Domain.Specifications;

namespace ludusGestao.Eventos.Application.UseCases.Local
{
    public class RemoverLocalUseCase
    {
        private readonly ILocalRepository _repository;

        public RemoverLocalUseCase(ILocalRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Executar(string id)
        {
            var entidade = await _repository.BuscarPorId(id);
            if (entidade == null)
                throw new ValidationException("Local não encontrado.");

            var disponivelSpec = new LocalDisponivelSpecification();
            if (!disponivelSpec.IsSatisfiedBy(entidade))
                throw new ValidationException(disponivelSpec.ErrorMessage);

            await _repository.Remover(entidade);
            return true;
        }
    }
} 