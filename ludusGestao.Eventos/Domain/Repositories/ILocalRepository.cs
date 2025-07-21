using ludusGestao.Eventos.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Repositories;
using LudusGestao.Shared.Domain.Common;

namespace ludusGestao.Eventos.Domain.Repositories
{
    public interface ILocalRepository : IRepository<Local>
    {
        Task<bool> ExistePorNome(string nome);
        Task<(IEnumerable<Local> Itens, int Total)> ListarPaginado(QueryParamsBase query);
    }
} 