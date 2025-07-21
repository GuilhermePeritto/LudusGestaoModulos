using ludusGestao.Eventos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Repositories;

namespace ludusGestao.Eventos.Domain.Repositories
{
    public interface ILocalRepository : IRepository<Local>
    {
        Task<bool> ExistePorNome(string nome);
    }
} 