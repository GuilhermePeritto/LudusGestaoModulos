using ludusGestao.Eventos.Domain.Local;

namespace ludusGestao.Eventos.Domain.Local.Specifications
{
    public class LocalAtivoSpecification
    {
        public string MensagemErro => "O local precisa estar ativo para ser atualizado.";
        public bool IsSatisfiedBy(Local local)
        {
            return local.Ativo;
        }
    }
} 