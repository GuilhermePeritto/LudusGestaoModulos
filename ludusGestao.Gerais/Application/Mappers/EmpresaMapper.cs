using ludusGestao.Gerais.Domain.DTOs.Empresa;
using ludusGestao.Gerais.Domain.Entities;
using LudusGestao.Shared.Application.Mappers;
using ludusGestao.Gerais.Domain.Enums;
using LudusGestao.Shared.Domain.ValueObjects;
using LudusGestao.Shared.Domain.Entities;

namespace ludusGestao.Gerais.Application.Mappers
{
    public class EmpresaMapper : MapeadorBase<CriarEmpresaDTO, AtualizarEmpresaDTO, Empresa, EmpresaDTO>
    {
        public override Empresa Mapear(CriarEmpresaDTO dto)
        {
            return new Empresa
            {
                Nome = dto.Nome,
                Cnpj = dto.Cnpj,
                Email = dto.Email,
                Endereco = dto.Endereco,
                Telefone = dto.Telefone,
                TenantId = dto.TenantId,
                Situacao = (SituacaoBase)(int)dto.Situacao
            };
        }

        public override void Mapear(AtualizarEmpresaDTO dto, Empresa empresa)
        {
            empresa.Nome = dto.Nome;
            empresa.Cnpj = dto.Cnpj;
            empresa.Email = dto.Email;
            empresa.Endereco = dto.Endereco;
            empresa.Telefone = dto.Telefone;
            empresa.Situacao = (SituacaoBase)(int)dto.Situacao;
        }

        public override EmpresaDTO Mapear(Empresa empresa)
        {
            return new EmpresaDTO
            {
                Id = empresa.Id,
                Nome = empresa.Nome,
                Cnpj = empresa.Cnpj,
                Email = empresa.Email,
                Endereco = empresa.Endereco,
                Telefone = empresa.Telefone,
                TenantId = empresa.TenantId,
                Situacao = (SituacaoEmpresa)empresa.Situacao
            };
        }
    }
} 