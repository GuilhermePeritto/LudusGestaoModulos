using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.Entities.Local.DTOs;

namespace ludusGestao.Eventos.Domain.Entities.Local.Interfaces
{
    public interface ILocalService
    {
        Task<LocalDTO> Criar(CriarLocalDTO dto);
        Task<IEnumerable<LocalDTO>> Listar();
        Task<LocalDTO> BuscarPorId(Guid id);
        Task<LocalDTO> Atualizar(Guid id, AtualizarLocalDTO dto);
        Task<bool> Remover(Guid id);
    }
} 