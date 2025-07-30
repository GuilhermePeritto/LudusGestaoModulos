using System.Linq.Expressions;
using ludusGestao.Eventos.Domain.Local;

namespace ludusGestao.Eventos.Domain.Local.Specifications
{
    public class LocalAtivoSpecification
    {
        public static Expression<Func<ludusGestao.Eventos.Domain.Local.Local, bool>> IsSatisfiedBy()
        {
            return local => local.Situacao == SituacaoLocal.Ativo;
        }
    }
} 