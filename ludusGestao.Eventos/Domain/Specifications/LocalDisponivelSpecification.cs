using ludusGestao.Eventos.Domain.Entities;

namespace ludusGestao.Eventos.Domain.Specifications
{
    public class LocalDisponivelSpecification : ISpecification<Local>
    {
        public string ErrorMessage => "Local não está disponível.";

        public bool IsSatisfiedBy(Local local)
        {
            return local != null && local.Ativo;
        }
    }
} 