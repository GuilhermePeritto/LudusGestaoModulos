using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace LudusGestao.Shared.Domain.ValueObjects
{
    [JsonConverter(typeof(EnderecoJsonConverter))]
    public class Endereco : ValueObjectBase
    {
        public string Rua { get; private set; }
        public string Numero { get; private set; }
        public string Bairro { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }
        public string Cep { get; private set; }

        // Construtor sem parâmetros para o EF
        public Endereco() { }

        public Endereco(string rua, string numero, string bairro, string cidade, string estado, string cep)
        {
            Rua = rua;
            Numero = numero;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
            Cep = cep;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Rua;
            yield return Numero;
            yield return Bairro;
            yield return Cidade;
            yield return Estado;
            yield return Cep;
        }
    }

    public class EnderecoJsonConverter : JsonConverter<Endereco>
    {
        public override Endereco Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();
            string rua = null, numero = null, bairro = null, cidade = null, estado = null, cep = null;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    break;
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propName = reader.GetString();
                    reader.Read();
                    switch (propName)
                    {
                        case "Rua": rua = reader.GetString(); break;
                        case "Numero": numero = reader.GetString(); break;
                        case "Bairro": bairro = reader.GetString(); break;
                        case "Cidade": cidade = reader.GetString(); break;
                        case "Estado": estado = reader.GetString(); break;
                        case "Cep": cep = reader.GetString(); break;
                    }
                }
            }
            return new Endereco(rua, numero, bairro, cidade, estado, cep);
        }

        public override void Write(Utf8JsonWriter writer, Endereco value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Rua", value.Rua);
            writer.WriteString("Numero", value.Numero);
            writer.WriteString("Bairro", value.Bairro);
            writer.WriteString("Cidade", value.Cidade);
            writer.WriteString("Estado", value.Estado);
            writer.WriteString("Cep", value.Cep);
            writer.WriteEndObject();
        }
    }
} 