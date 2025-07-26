using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Eventos.Domain.Local.DTOs;

namespace ludusGestao.Eventos.Domain.Local.Interfaces
{
    public interface ILocalService
    {
        Task<LocalDTO> Criar(CriarLocalDTO dto);
        Task<LocalDTO> Atualizar(Guid id, AtualizarLocalDTO dto);
        Task<bool> Remover(Guid id);
        Task<LocalDTO> BuscarPorId(Guid id);
        Task<IEnumerable<LocalDTO>> Listar(QueryParamsBase query);
    }
} 