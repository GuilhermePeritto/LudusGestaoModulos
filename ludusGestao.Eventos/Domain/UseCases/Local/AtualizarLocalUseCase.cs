public class AtualizarLocalUseCase : BaseUseCase, IAtualizarLocalUseCase
{
    private readonly ILocalWriteProvider _writeProvider;
    public AtualizarLocalUseCase(ILocalWriteProvider writeProvider, INotificador notificador)
        : base(notificador)
    {
        _writeProvider = writeProvider;
    }

    public async Task<Local> Executar(Local local)
    {
        if (!ExecutarValidacao(new AtualizarLocalValidation(), local))
            return local;
            
        local.MarcarAlterado();
        await _writeProvider.Atualizar(local);
        await _writeProvider.SalvarAlteracoes();
        return local;
    }
} 