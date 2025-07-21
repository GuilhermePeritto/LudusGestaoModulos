using ludusGestao.Gerais.Domain.Repositories;

namespace ludusGestao.Gerais.Domain.Specifications.Empresa
{
    public class CnpjUnicoSpecification
    {
        private readonly IEmpresaRepository _repository;
        public CnpjUnicoSpecification(IEmpresaRepository repository)
        {
            _repository = repository;
        }
        // Apenas verifica unicidade, pois formato já é garantido pelo ValueObject
        public bool IsSatisfiedBy(string cnpj)
        {
            return !_repository.ExistePorCnpj(cnpj).Result;
        }
        public string ErrorMessage => "Já existe uma empresa com este CNPJ.";
    }
} 