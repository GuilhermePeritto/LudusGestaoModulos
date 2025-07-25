using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Domain.Providers;

public class BuscarLocalPorIdUseCase : BaseUseCase
{
    private readonly ILocalReadProvider _readProvider;
    public BuscarLocalPorIdUseCase(ILocalReadProvider readProvider, INotificador notificador)
        : base(notificador)
    {
        _readProvider = readProvider;
    }

    public async Task<Local> Executar(Guid id)
    {
        return await _readProvider.Buscar(QueryParamsHelper.BuscarPorId(id));
    }
} 