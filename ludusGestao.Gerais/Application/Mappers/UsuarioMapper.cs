using ludusGestao.Gerais.Domain.DTOs.Usuario;
using ludusGestao.Gerais.Domain.Entities;
using LudusGestao.Shared.Application.Mappers;
using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Gerais.Application.Mappers
{
    public class UsuarioMapper : MapeadorBase<CriarUsuarioDTO, AtualizarUsuarioDTO, Usuario, UsuarioDTO>
    {
        public override Usuario Mapear(CriarUsuarioDTO dto)
        {
            return new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Telefone = dto.Telefone,
                Cargo = dto.Cargo,
                EmpresaId = dto.EmpresaId,
                Foto = dto.Foto,
                Senha = dto.Senha,
                TenantId = dto.TenantId,
                Endereco = dto.Endereco
            };
        }

        public override void Mapear(AtualizarUsuarioDTO dto, Usuario usuario)
        {
            usuario.Nome = dto.Nome;
            usuario.Telefone = dto.Telefone;
            usuario.Cargo = dto.Cargo;
            usuario.Foto = dto.Foto;
            usuario.Email = dto.Email;
            usuario.Endereco = dto.Endereco;
        }

        public override UsuarioDTO Mapear(Usuario usuario)
        {
            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                Cargo = usuario.Cargo,
                EmpresaId = usuario.EmpresaId,
                Endereco = usuario.Endereco,
                Situacao = usuario.Situacao,
                UltimoAcesso = usuario.UltimoAcesso,
                Foto = usuario.Foto,
                TenantId = usuario.TenantId
            };
        }
    }
} 