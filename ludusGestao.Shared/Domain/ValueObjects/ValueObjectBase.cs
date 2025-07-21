using System;
using System.Collections.Generic;
using System.Linq;

namespace LudusGestao.Shared.Domain.ValueObjects
{
    public abstract class ValueObjectBase
    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (obj is not ValueObjectBase other)
                return false;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                    current * 23 + (obj?.GetHashCode() ?? 0));
        }
    }
} 