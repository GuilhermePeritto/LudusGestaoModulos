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
        return await _readProvider.BuscarPorId(id);
    }
} 