using LudusGestao.Shared.Domain.Entities;
using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Eventos.Domain.Entities.Local
{
    public enum SituacaoLocal
    {
        Ativo = SituacaoBase.Ativo,
        Inativo = SituacaoBase.Inativo
    }

    public class Local : EntidadeBase
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public Endereco Endereco { get; private set; }
        public Telefone Telefone { get; private set; }
        public SituacaoLocal Situacao { get; private set; }

        // Construtor privado para garantir criação via factory method
        private Local() { }

        // Factory method para criação
        public static Local Criar(string nome, string descricao, Endereco endereco, Telefone telefone)
        {
            return new Local
            {
                Nome = nome,
                Descricao = descricao,
                Endereco = endereco,
                Telefone = telefone,
                Situacao = SituacaoLocal.Ativo
            };
        }

        // Métodos de negócio
        public void Atualizar(string nome, string descricao, Endereco endereco, Telefone telefone)
        {
            Nome = nome;
            Descricao = descricao;
            Endereco = endereco;
            Telefone = telefone;
            MarcarAlterado();
        }

        public void Ativar()
        {
            Situacao = SituacaoLocal.Ativo;
            MarcarAlterado();
        }

        public void Inativar()
        {
            Situacao = SituacaoLocal.Inativo;
            MarcarAlterado();
        }

        public bool EstaAtivo()
        {
            return Situacao == SituacaoLocal.Ativo;
        }
    }
}