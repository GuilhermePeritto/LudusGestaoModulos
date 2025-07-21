using ludusGestao.Gerais.Domain.DTOs.Filial;
using ludusGestao.Gerais.Domain.Entities;
using LudusGestao.Shared.Application.Mappers;
using ludusGestao.Gerais.Domain.Enums;
using LudusGestao.Shared.Domain.ValueObjects;
using LudusGestao.Shared.Domain.Entities;

namespace ludusGestao.Gerais.Application.Mappers
{
    public class FilialMapper : MapeadorBase<CriarFilialDTO, AtualizarFilialDTO, Filial, FilialDTO>
    {
        public override Filial Mapear(CriarFilialDTO dto)
        {
            return new Filial
            {
                Nome = dto.Nome,
                Codigo = dto.Codigo,
                Endereco = dto.Endereco,
                Telefone = dto.Telefone,
                Email = dto.Email,
                Cnpj = dto.Cnpj,
                Responsavel = dto.Responsavel,
                Situacao = (SituacaoBase)(int)dto.Situacao,
                DataAbertura = dto.DataAbertura,
                TenantId = dto.TenantId,
                EmpresaId = dto.EmpresaId
            };
        }

        public override void Mapear(AtualizarFilialDTO dto, Filial filial)
        {
            filial.Nome = dto.Nome;
            filial.Codigo = dto.Codigo;
            filial.Endereco = dto.Endereco;
            filial.Telefone = dto.Telefone;
            filial.Email = dto.Email;
            filial.Cnpj = dto.Cnpj;
            filial.Responsavel = dto.Responsavel;
            filial.Situacao = (SituacaoBase)(int)dto.Situacao;
            filial.DataAbertura = dto.DataAbertura;
        }

        public override FilialDTO Mapear(Filial filial)
        {
            return new FilialDTO
            {
                Id = filial.Id,
                Nome = filial.Nome,
                Codigo = filial.Codigo,
                Endereco = filial.Endereco,
                Telefone = filial.Telefone,
                Email = filial.Email,
                Cnpj = filial.Cnpj,
                Responsavel = filial.Responsavel,
                Situacao = (SituacaoFilial)filial.Situacao,
                DataAbertura = filial.DataAbertura,
                TenantId = filial.TenantId,
                EmpresaId = filial.EmpresaId
            };
        }
    }
} 