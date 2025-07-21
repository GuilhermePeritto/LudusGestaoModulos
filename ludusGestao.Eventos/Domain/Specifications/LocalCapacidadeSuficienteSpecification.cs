using ludusGestao.Eventos.Domain.Entities;

namespace ludusGestao.Eventos.Domain.Specifications
{
    public class LocalCapacidadeSuficienteSpecification : ISpecification<Local>
    {
        private readonly int _capacidadeNecessaria;
        public string ErrorMessage => $"Local não possui capacidade suficiente. Necessária: {_capacidadeNecessaria}.";

        public LocalCapacidadeSuficienteSpecification(int capacidadeNecessaria)
        {
            _capacidadeNecessaria = capacidadeNecessaria;
        }

        public bool IsSatisfiedBy(Local local)
        {
            return local != null && local.Capacidade >= _capacidadeNecessaria;
        }
    }
} 