using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LudusGestao.Shared.Domain.ValueObjects
{
    public class Email : ValueObjectBase
    {
        public string Valor { get; }

        public Email(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("Email não pode ser vazio.");
            if (!Regex.IsMatch(valor, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Formato de email inválido.");
            Valor = valor;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Valor.ToLowerInvariant();
        }

        public override string ToString() => Valor;
    }
} 