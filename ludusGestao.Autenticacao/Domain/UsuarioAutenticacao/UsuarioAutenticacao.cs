using System;
using LudusGestao.Shared.Domain.Entities;
using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Autenticacao.Domain.UsuarioAutenticacao
{
    public class UsuarioAutenticacao : EntidadeBase
    {
        public UsuarioAutenticacao() { }
        
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public bool Ativo { get; private set; }
        public int TenantId { get; private set; }

        public static UsuarioAutenticacao Criar(string email, string senha)
        {
            return new UsuarioAutenticacao
            {
                Email = email,
                Senha = senha,
                Ativo = true
            };
        }

        public void Atualizar(string email, string senha)
        {
            Email = email;
            Senha = senha;
        }

        public void AlterarSenha(string novaSenha)
        {
            Senha = novaSenha;
        }

        public void Desativar()
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
} 