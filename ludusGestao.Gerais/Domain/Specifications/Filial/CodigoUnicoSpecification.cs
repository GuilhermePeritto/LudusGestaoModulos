using ludusGestao.Gerais.Domain.Providers;

namespace ludusGestao.Gerais.Domain.Specifications.Filial
{
    public class CodigoUnicoSpecification
    {
        private readonly IFilialReadProvider _readProvider;
        public CodigoUnicoSpecification(IFilialReadProvider readProvider)
        {
            _readProvider = readProvider;
        }
        // Apenas verifica unicidade
        public bool IsSatisfiedBy(string codigo)
        {
            return !_readProvider.ExistePorCodigo(codigo).Result;
        }
        public string ErrorMessage => "Já existe uma filial com este código.";
    }
} 