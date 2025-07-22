using ludusGestao.Gerais.Domain.Providers;

namespace ludusGestao.Gerais.Domain.Specifications.Usuario
{
    public class EmailUnicoSpecification
    {
        private readonly IUsuarioReadProvider _readProvider;
        public EmailUnicoSpecification(IUsuarioReadProvider readProvider)
        {
            _readProvider = readProvider;
        }
        // Apenas verifica unicidade, pois formato já é garantido pelo ValueObject
        public bool IsSatisfiedBy(string email)
        {
            return !_readProvider.ExistePorEmail(email).Result;
        }
        public string ErrorMessage => "Já existe um usuário com este e-mail.";
    }
} 