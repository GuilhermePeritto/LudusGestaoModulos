using LudusGestao.Shared.Domain.Providers;

public class RemoverLocalUseCase : BaseUseCase
{
    private readonly ILocalWriteProvider _writeProvider;
    private readonly ILocalReadProvider _readProvider;
    public RemoverLocalUseCase(ILocalWriteProvider writeProvider, ILocalReadProvider readProvider, INotificador notificador)
        : base(notificador)
    {
        _writeProvider = writeProvider;
        _readProvider = readProvider;
    }

    public async Task<bool> Executar(Guid id)
    {
        var query = QueryParamsHelper.BuscarPorId(id);
        var existente = (await _readProvider.Listar(query)).FirstOrDefault();
        if (existente == null)
        {
            Notificar("Local não encontrado para remoção.");
            return false;
        }
        await _writeProvider.Remover(id);
        await _writeProvider.SalvarAlteracoes();
        return true;
    }
} 