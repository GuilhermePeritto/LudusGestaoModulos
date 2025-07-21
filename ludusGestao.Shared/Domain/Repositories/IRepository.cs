using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Entities;

namespace LudusGestao.Shared.Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : EntidadeBase
    {
        Task<TEntity> BuscarPorId(string id);
        Task Adicionar(TEntity entidade);
        Task Atualizar(TEntity entidade);
        Task Remover(TEntity entidade);
        Task<IEnumerable<TEntity>> ListarTodos();
    }
} 