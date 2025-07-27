using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao;

namespace ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Specifications
{
    public class UsuarioAtivoSpecification
    {
        public string MensagemErro => "O usuário precisa estar ativo para realizar a autenticação.";
        
        public bool IsSatisfiedBy(UsuarioAutenticacao usuario)
        {
            return usuario.Ativo;
        }
    }
} 