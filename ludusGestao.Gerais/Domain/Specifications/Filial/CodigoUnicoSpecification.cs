using ludusGestao.Gerais.Domain.Repositories;

namespace ludusGestao.Gerais.Domain.Specifications.Filial
{
    public class CodigoUnicoSpecification
    {
        private readonly IFilialRepository _repository;
        public CodigoUnicoSpecification(IFilialRepository repository)
        {
            _repository = repository;
        }
        // Apenas verifica unicidade
        public bool IsSatisfiedBy(string codigo)
        {
            return !_repository.ExistePorCodigo(codigo).Result;
        }
        public string ErrorMessage => "Já existe uma filial com este código.";
    }
} 