using LudusGestao.Shared.Domain.Common;

public interface IListarLocaisUseCase
{
    Task<IEnumerable<Local>> Executar(QueryParamsBase queryParams);
} 