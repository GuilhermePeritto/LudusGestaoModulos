using LudusGestao.Shared.Domain.Entities;
using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Eventos.Domain.Entities.Local
{
    public enum SituacaoLocal
    {
        Ativo = 1,
        Inativo = 2
    }

    public class Local : EntidadeBase
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public Endereco Endereco { get; private set; }
        public Telefone Telefone { get; private set; }
        public SituacaoLocal Situacao { get; private set; }

        // Construtor para Entity Framework
        protected Local() { }

        public Local(string nome, string descricao, Endereco endereco, Telefone telefone)
        {
            Nome = nome;
            Descricao = descricao;
            Endereco = endereco;
            Telefone = telefone;
            Situacao = SituacaoLocal.Ativo;
        }

        public void Atualizar(string nome, string descricao, Endereco endereco, Telefone telefone)
        {
            Nome = nome;
            Descricao = descricao;
            Endereco = endereco;
            Telefone = telefone;
        }

        public void Ativar()
        {
            Situacao = SituacaoLocal.Ativo;
        }

        public void Inativar()
        {
            Situacao = SituacaoLocal.Inativo;
        }
    }
}