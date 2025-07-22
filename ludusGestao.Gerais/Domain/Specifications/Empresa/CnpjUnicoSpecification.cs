using ludusGestao.Gerais.Domain.Providers;

namespace ludusGestao.Gerais.Domain.Specifications.Empresa
{
    public class CnpjUnicoSpecification
    {
        private readonly IEmpresaReadProvider _readProvider;
        public CnpjUnicoSpecification(IEmpresaReadProvider readProvider)
        {
            _readProvider = readProvider;
        }
        // Apenas verifica unicidade, pois formato já é garantido pelo ValueObject
        public bool IsSatisfiedBy(string cnpj)
        {
            return !_readProvider.ExistePorCnpj(cnpj).Result;
        }
        public string ErrorMessage => "Já existe uma empresa com este CNPJ.";
    }
} 