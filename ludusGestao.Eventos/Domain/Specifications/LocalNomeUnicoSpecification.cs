using ludusGestao.Eventos.Domain.Repositories;

namespace ludusGestao.Eventos.Domain.Specifications
{
    public class LocalNomeUnicoSpecification : ISpecification<string>
    {
        private readonly ILocalRepository _repository;
        public string ErrorMessage => "Já existe um local com esse nome.";

        public LocalNomeUnicoSpecification(ILocalRepository repository)
        {
            _repository = repository;
        }

        public bool IsSatisfiedBy(string nome)
        {
            // Para uso síncrono em FluentValidation, mas idealmente use async se possível
            return !_repository.ExistePorNome(nome).GetAwaiter().GetResult();
        }
    }
} 