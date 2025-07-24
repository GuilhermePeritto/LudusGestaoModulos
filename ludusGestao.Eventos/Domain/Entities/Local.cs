using System;
using LudusGestao.Shared.Domain.Entities;
using LudusGestao.Shared.Domain.ValueObjects;

public class Local : EntidadeBase
{
    public Local() { }
    public string Nome { get; private set; }
    public Endereco Endereco { get; private set; }
    public int Capacidade { get; private set; }
    public bool Ativo { get; private set; }
    public int TenantId { get; private set; }

    public static Local Criar(string nome, string rua, string numero, string bairro, string cidade, string estado, string cep, int capacidade)
    {
        return new Local
        {
            Nome = nome,
            Endereco = new Endereco(rua, numero, bairro, cidade, estado, cep),
            Capacidade = capacidade,
            Ativo = true,
        };
    }

    public void Atualizar(string nome, string rua, string numero, string bairro, string cidade, string estado, string cep, int capacidade)
    {
        Nome = nome;
        Endereco = new Endereco(rua, numero, bairro, cidade, estado, cep);
        Capacidade = capacidade;
    }

    public void Remover()
    {
        Ativo = false;
    }

    public void Reativar()
    {
        Ativo = true;
    }

    public void AlterarTenant(int tenantId)
    {
        TenantId = tenantId;
    }
}