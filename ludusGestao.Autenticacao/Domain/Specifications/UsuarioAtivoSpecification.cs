using ludusGestao.Autenticacao.Domain.Entities;

namespace ludusGestao.Autenticacao.Domain.Specifications
{
    public class UsuarioAtivoSpecification
    {
        public bool IsSatisfiedBy(UsuarioAutenticacao usuario)
        {
            return usuario.Ativo;
        }
    }
} 