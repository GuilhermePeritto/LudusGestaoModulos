public class ListarLocaisUseCase : BaseUseCase
{
    private readonly ILocalReadProvider _readProvider;
    public ListarLocaisUseCase(ILocalReadProvider readProvider, INotificador notificador)
        : base(notificador)
    {
        _readProvider = readProvider;
    }

    public async Task<IEnumerable<Local>> Executar()
    {
        return await _readProvider.ListarTodos();
    }
} 