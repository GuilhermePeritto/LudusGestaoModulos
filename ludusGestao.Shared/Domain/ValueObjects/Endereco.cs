using System.Collections.Generic;

namespace LudusGestao.Shared.Domain.ValueObjects
{
    public class Endereco : ValueObjectBase
    {
        public string Rua { get; }
        public string Numero { get; }
        public string Bairro { get; }
        public string Cidade { get; }
        public string Estado { get; }
        public string Cep { get; }

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
} 