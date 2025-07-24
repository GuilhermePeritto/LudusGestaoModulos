using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LudusGestao.Shared.Domain.ValueObjects;

namespace LudusGestao.Shared.Domain.ValueObjects
{
    public class Cpf : ValueObjectBase
    {
        public string Numero { get; private set; }

        // Construtor sem parâmetros para o EF
        public Cpf() { }

        public Cpf(string numero)
        {
            if (!EhCpfValido(numero))
                throw new ArgumentException("CPF inválido.");
            Numero = numero;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Numero;
        }

        private bool EhCpfValido(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;
            cpf = Regex.Replace(cpf, "[^0-9]", "");
            if (cpf.Length != 11) return false;
            if (new string(cpf[0], cpf.Length) == cpf) return false;
            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            string digito = resto.ToString();
            tempCpf += digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            digito += resto.ToString();
            return cpf.EndsWith(digito);
        }
    }
} 