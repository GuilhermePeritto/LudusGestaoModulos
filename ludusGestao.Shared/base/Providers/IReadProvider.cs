using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.QueryParams;

namespace LudusGestao.Shared.Domain.Providers
{
    public interface IReadProvider<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> Listar();
        Task<IEnumerable<object>> Listar(QueryParamsBase queryParams);
        Task<TEntity?> Buscar(QueryParamsBase queryParams);
    }
} 