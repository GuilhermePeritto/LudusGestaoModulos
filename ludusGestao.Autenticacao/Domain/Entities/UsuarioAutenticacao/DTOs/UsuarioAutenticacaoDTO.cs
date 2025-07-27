using System;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao;

namespace ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.DTOs
{
    public class UsuarioAutenticacaoDTO
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public bool Ativo { get; private set; }
        public int TenantId { get; private set; }

        public static UsuarioAutenticacaoDTO Criar(UsuarioAutenticacao usuario)
        {
            return new UsuarioAutenticacaoDTO
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Ativo = usuario.Ativo,
                TenantId = usuario.TenantId
            };
        }
    }
} 