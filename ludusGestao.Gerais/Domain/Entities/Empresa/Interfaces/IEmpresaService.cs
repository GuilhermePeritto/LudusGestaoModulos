using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.QueryParams;
using ludusGestao.Gerais.Domain.Empresa.DTOs;

namespace ludusGestao.Gerais.Domain.Empresa.Interfaces
{
    public interface IEmpresaService
    {
        Task<EmpresaDTO> Criar(CriarEmpresaDTO dto);
        Task<EmpresaDTO> Atualizar(Guid id, AtualizarEmpresaDTO dto);
        Task<bool> Remover(Guid id);
        Task<EmpresaDTO> BuscarPorId(Guid id);
        Task<IEnumerable<EmpresaDTO>> Listar(QueryParamsBase query);
        Task<bool> Ativar(Guid id);
        Task<bool> Desativar(Guid id);
    }
} 