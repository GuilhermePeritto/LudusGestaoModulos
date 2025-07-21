using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LudusGestao.Shared.Domain.ValueObjects
{
    public class Cnpj : ValueObjectBase
    {
        public string Valor { get; }

        public Cnpj(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("CNPJ não pode ser vazio.");
            valor = Regex.Replace(valor, "[^0-9]", "");
            if (valor.Length != 14 || !EhCnpjValido(valor))
                throw new ArgumentException("CNPJ inválido.");
            Valor = valor;
        }

        private bool EhCnpjValido(string cnpj)
        {
            int[] mult1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mult2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;
            for (int i = 0; i < 12; i++) soma += int.Parse(tempCnpj[i].ToString()) * mult1[i];
            int resto = (soma % 11);
            if (resto < 2) resto = 0; else resto = 11 - resto;
            string digito = resto.ToString();
            tempCnpj += digito;
            soma = 0;
            for (int i = 0; i < 13; i++) soma += int.Parse(tempCnpj[i].ToString()) * mult2[i];
            resto = (soma % 11);
            if (resto < 2) resto = 0; else resto = 11 - resto;
            digito += resto.ToString();
            return cnpj.EndsWith(digito);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Valor;
        }

        public override string ToString() => Valor;
    }
} 