using LudusGestao.Shared.Domain.Common;

public interface ILocalReadProvider
{
    Task<IEnumerable<Local>> Listar();
    Task<IEnumerable<Local>> Listar(QueryParamsBase queryParams);
    Task<Local> Buscar(QueryParamsBase queryParams);
}
