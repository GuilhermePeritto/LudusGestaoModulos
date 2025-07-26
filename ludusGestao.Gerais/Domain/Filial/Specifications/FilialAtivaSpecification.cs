using ludusGestao.Gerais.Domain.Filial;

namespace ludusGestao.Gerais.Domain.Filial.Specifications
{
    public class FilialAtivaSpecification
    {
        public string MensagemErro => "A filial precisa estar ativa para ser atualizada.";
        
        public bool IsSatisfiedBy(Filial filial)
        {
            return filial.EstaAtiva();
        }
    }
} 