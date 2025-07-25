using LudusGestao.Shared.Domain.Common;

public class ListarLocaisUseCase : BaseUseCase
{
    private readonly ILocalReadProvider _readProvider;
    public ListarLocaisUseCase(ILocalReadProvider readProvider, INotificador notificador)
        : base(notificador)
    {
        _readProvider = readProvider;
    }

    public async Task<IEnumerable<Local>> Executar(QueryParamsBase queryParams)
    {
        return await _readProvider.Listar(queryParams);
    }
} 