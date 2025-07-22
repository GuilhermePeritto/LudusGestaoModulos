using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace LudusGestao.Shared.Domain.ValueObjects
{
    [JsonConverter(typeof(CpfJsonConverter))]
    public class Cpf : ValueObjectBase
    {
        public string Valor { get; private set; }

        // Construtor sem parâmetros para o EF
        public Cpf() { }

        public Cpf(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("CPF não pode ser vazio.");
            valor = Regex.Replace(valor, "[^0-9]", "");
            if (valor.Length != 11 || !EhCpfValido(valor))
                throw new ArgumentException("CPF inválido.");
            Valor = valor;
        }

        private bool EhCpfValido(string cpf)
        {
            if (new string(cpf[0], 11) == cpf) return false;
            int[] mult1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] mult2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;
            for (int i = 0; i < 9; i++) soma += int.Parse(tempCpf[i].ToString()) * mult1[i];
            int resto = soma % 11;
            if (resto < 2) resto = 0; else resto = 11 - resto;
            string digito = resto.ToString();
            tempCpf += digito;
            soma = 0;
            for (int i = 0; i < 10; i++) soma += int.Parse(tempCpf[i].ToString()) * mult2[i];
            resto = soma % 11;
            if (resto < 2) resto = 0; else resto = 11 - resto;
            digito += resto.ToString();
            return cpf.EndsWith(digito);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Valor;
        }

        public override string ToString() => Valor;
    }

    public class CpfJsonConverter : JsonConverter<Cpf>
    {
        public override Cpf Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("CPF não pode ser nulo ou vazio.");
            return new Cpf(value);
        }

        public override void Write(Utf8JsonWriter writer, Cpf value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Valor);
        }
    }
} 