using ludusGestao.Gerais.Domain.Usuario;

namespace ludusGestao.Gerais.Domain.Usuario.Specifications
{
    public class UsuarioAtivoSpecification
    {
        public string MensagemErro => "O usuário precisa estar ativo para ser atualizado.";
        
        public bool IsSatisfiedBy(Usuario usuario)
        {
            return usuario.EstaAtivo();
        }
    }
} 