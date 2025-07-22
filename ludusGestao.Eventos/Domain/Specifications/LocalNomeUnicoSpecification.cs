using ludusGestao.Eventos.Domain.Providers;

namespace ludusGestao.Eventos.Domain.Specifications
{
    public class LocalNomeUnicoSpecification : ISpecification<string>
    {
        private readonly ILocalReadProvider _readProvider;
        public string ErrorMessage => "Já existe um local com esse nome.";

        public LocalNomeUnicoSpecification(ILocalReadProvider readProvider)
        {
            _readProvider = readProvider;
        }

        public bool IsSatisfiedBy(string nome)
        {
            // Para uso síncrono em FluentValidation, mas idealmente use async se possível
            return !_readProvider.ExistePorNome(nome).GetAwaiter().GetResult();
        }
    }
} 