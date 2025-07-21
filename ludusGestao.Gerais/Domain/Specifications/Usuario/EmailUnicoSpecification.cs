using ludusGestao.Gerais.Domain.Repositories;

namespace ludusGestao.Gerais.Domain.Specifications.Usuario
{
    public class EmailUnicoSpecification
    {
        private readonly IUsuarioRepository _repository;
        public EmailUnicoSpecification(IUsuarioRepository repository)
        {
            _repository = repository;
        }
        // Apenas verifica unicidade, pois formato já é garantido pelo ValueObject
        public bool IsSatisfiedBy(string email)
        {
            return !_repository.ExistePorEmail(email).Result;
        }
        public string ErrorMessage => "Já existe um usuário com este e-mail.";
    }
} 