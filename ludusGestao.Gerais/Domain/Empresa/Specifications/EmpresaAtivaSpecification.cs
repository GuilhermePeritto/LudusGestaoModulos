using ludusGestao.Gerais.Domain.Empresa;

namespace ludusGestao.Gerais.Domain.Empresa.Specifications
{
    public class EmpresaAtivaSpecification
    {
        public string MensagemErro => "A empresa precisa estar ativa para ser atualizada.";
        
        public bool IsSatisfiedBy(Empresa empresa)
        {
            return empresa.EstaAtiva();
        }
    }
} 