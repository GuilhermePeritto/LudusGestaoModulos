using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace LudusGestao.Shared.Domain.ValueObjects
{
    [JsonConverter(typeof(EmailJsonConverter))]
    public class Email : ValueObjectBase
    {
        public string Valor { get; private set; }

        // Construtor sem parâmetros para o EF
        public Email() { }

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

    public class EmailJsonConverter : JsonConverter<Email>
    {
        public override Email Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email não pode ser nulo ou vazio.");
            return new Email(value);
        }

        public override void Write(Utf8JsonWriter writer, Email value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Valor);
        }
    }
} 