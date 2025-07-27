using System.Linq.Expressions;
using ludusGestao.Eventos.Domain.Entities.Local;

namespace ludusGestao.Eventos.Domain.Entities.Local.Specifications
{
    public class LocalAtivoSpecification
    {
        public static Expression<Func<ludusGestao.Eventos.Domain.Entities.Local.Local, bool>> IsSatisfiedBy()
        {
            return local => local.Situacao == SituacaoLocal.Ativo;
        }
    }
} 