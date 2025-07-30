using System;
using ludusGestao.Gerais.Domain.Usuario;
using LudusGestao.Shared.Domain.Entities;

namespace ludusGestao.Gerais.Domain.Usuario.DTOs
{
    public class UsuarioDTO : DTOBase
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Telefone { get; private set; }
        public string Cargo { get; private set; }
        public Guid EmpresaId { get; private set; }
        public string Situacao { get; private set; }
        public DateTime UltimoAcesso { get; private set; }
        public string? Foto { get; private set; }
        public string Rua { get; private set; }
        public string Numero { get; private set; }
        public string Bairro { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }
        public string Cep { get; private set; }

        // Construtor padrão necessário para o DTOBase
        public UsuarioDTO()
        {
        }

        public static UsuarioDTO Criar(Usuario usuario)
        {
            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email.Endereco,
                Telefone = usuario.Telefone.Numero,
                Cargo = usuario.Cargo,
                EmpresaId = usuario.EmpresaId,
                Situacao = usuario.Situacao.ToString(),
                UltimoAcesso = usuario.UltimoAcesso,
                Foto = usuario.Foto,
                Rua = usuario.Endereco.Rua,
                Numero = usuario.Endereco.Numero,
                Bairro = usuario.Endereco.Bairro,
                Cidade = usuario.Endereco.Cidade,
                Estado = usuario.Endereco.Estado,
                Cep = usuario.Endereco.Cep,
                TenantId = usuario.TenantId,
                DataCriacao = usuario.DataCriacao,
                DataAlteracao = usuario.DataAlteracao
            };
        }
    }
} 