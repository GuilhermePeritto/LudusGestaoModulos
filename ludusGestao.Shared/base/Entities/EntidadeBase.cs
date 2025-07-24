using System;

namespace LudusGestao.Shared.Domain.Entities
{
    public abstract class EntidadeBase
    {
        public Guid Id { get; protected set; }
        public DateTime DataCriacao { get; protected set; }
        public DateTime? DataAlteracao { get; protected set; }
        public int TenantId { get; set; }

        protected EntidadeBase()
        {
            Id = Guid.NewGuid();
            DataCriacao = DateTime.UtcNow;
        }

        public void MarcarAlterado()
        {
            DataAlteracao = DateTime.UtcNow;
        }

        public override bool Equals(object obj)
        {
            if (obj is not EntidadeBase other)
                return false;
            return Id == other.Id;
        }

        public override int GetHashCode() => Id.GetHashCode();
    }
} 