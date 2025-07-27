using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.Entities.Local.DTOs;

namespace ludusGestao.Eventos.Domain.Entities.Local.Interfaces
{
    public interface ILocalService
    {
        Task<ludusGestao.Eventos.Domain.Entities.Local.Local> Criar(CriarLocalDTO dto);
        Task<IEnumerable<ludusGestao.Eventos.Domain.Entities.Local.Local>> Listar();
        Task<ludusGestao.Eventos.Domain.Entities.Local.Local> BuscarPorId(Guid id);
        Task<ludusGestao.Eventos.Domain.Entities.Local.Local> Atualizar(Guid id, AtualizarLocalDTO dto);
        Task Remover(Guid id);
    }
} 