using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LudusGestao.Shared.Domain.ValueObjects;

namespace LudusGestao.Shared.Domain.ValueObjects
{
    public class Telefone : ValueObjectBase
    {
        public string Numero { get; private set; }

        // Construtor sem parâmetros para o EF
        public Telefone() { }

        public Telefone(string numero)
        {
            if (!EhTelefoneValido(numero))
                throw new ArgumentException("Telefone inválido.");
            Numero = numero;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Numero;
        }

        private bool EhTelefoneValido(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone)) return false;
            telefone = Regex.Replace(telefone, "[^0-9]", "");
            // Aceita formatos de telefone brasileiro (com DDD, 10 ou 11 dígitos)
            return telefone.Length == 10 || telefone.Length == 11;
        }
    }
} 