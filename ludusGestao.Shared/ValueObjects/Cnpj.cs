using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LudusGestao.Shared.Domain.ValueObjects;

namespace LudusGestao.Shared.Domain.ValueObjects
{
    public class Cnpj : ValueObjectBase
    {
        public string Numero { get; private set; }

        // Construtor sem parâmetros para o EF
        public Cnpj() { }

        public Cnpj(string numero)
        {
            if (!EhCnpjValido(numero))
                throw new ArgumentException("CNPJ inválido.");
            Numero = numero;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Numero;
        }

        private bool EhCnpjValido(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj)) return false;
            cnpj = Regex.Replace(cnpj, "[^0-9]", "");
            if (cnpj.Length != 14) return false;
            int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            int resto = (soma % 11);
            resto = resto < 2 ? 0 : 11 - resto;
            string digito = resto.ToString();
            tempCnpj += digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            resto = resto < 2 ? 0 : 11 - resto;
            digito += resto.ToString();
            return cnpj.EndsWith(digito);
        }
    }
} 