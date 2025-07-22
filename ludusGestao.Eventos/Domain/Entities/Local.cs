using System;
using LudusGestao.Shared.Domain.Entities;
using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Eventos.Domain.Entities
{
    public class Local : EntidadeBase, IEntidadeTenant
    {
        public string Nome { get; private set; }
        public Endereco Endereco { get; private set; }
        public int Capacidade { get; private set; }
        public bool Ativo { get; private set; }
        public int TenantId { get; set; }

        private Local() { }

        public static Local Criar(string nome, Endereco endereco, int capacidade)
        {
            return new Local
            {
                Id = Guid.NewGuid(),
                Nome = nome,
                Endereco = endereco,
                Capacidade = capacidade,
                Ativo = true
            };
        }

        public void Atualizar(string nome, Endereco endereco, int capacidade)
        {
            Nome = nome;
            Endereco = endereco;
            Capacidade = capacidade;
            Ativo = true;
        }
    }
} 