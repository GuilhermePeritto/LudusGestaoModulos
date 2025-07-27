using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;

namespace LudusGestao.Shared.Domain.Providers
{
    public interface IReadProvider<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> Listar();
        Task<IEnumerable<TEntity>> Listar(QueryParamsBase queryParams);
        Task<TEntity> Buscar(QueryParamsBase queryParams);
    }
} 