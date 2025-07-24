using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LudusGestao.Shared.Domain.ValueObjects;

namespace LudusGestao.Shared.Domain.ValueObjects
{
    public class Email : ValueObjectBase
    {
        public string Endereco { get; private set; }

        // Construtor sem parâmetros para o EF
        public Email() { }

        public Email(string endereco)
        {
            if (!EhEmailValido(endereco))
                throw new ArgumentException("E-mail inválido.");
            Endereco = endereco;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Endereco;
        }

        private bool EhEmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }
    }
} 