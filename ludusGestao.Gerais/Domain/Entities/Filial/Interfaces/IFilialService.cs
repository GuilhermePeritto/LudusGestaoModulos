using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.QueryParams;
using ludusGestao.Gerais.Domain.Filial.DTOs;

namespace ludusGestao.Gerais.Domain.Filial.Interfaces
{
    public interface IFilialService
    {
        Task<FilialDTO> Criar(CriarFilialDTO dto);
        Task<FilialDTO> Atualizar(Guid id, AtualizarFilialDTO dto);
        Task<bool> Remover(Guid id);
        Task<FilialDTO> BuscarPorId(Guid id);
        Task<IEnumerable<FilialDTO>> Listar(QueryParamsBase query);
        Task<bool> Ativar(Guid id);
        Task<bool> Desativar(Guid id);
    }
} 