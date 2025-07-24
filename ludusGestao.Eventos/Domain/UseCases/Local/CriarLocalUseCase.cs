public class CriarLocalUseCase : BaseUseCase
{
    private readonly ILocalWriteProvider _provider;
    public CriarLocalUseCase(ILocalWriteProvider provider, INotificador notificador)
        : base(notificador)
    {
        _provider = provider;
    }

    public async Task<Local> Executar(Local local)
    {
        if (!ExecutarValidacao(new CriarLocalValidation(), local))
            return null;

        await _provider.Adicionar(local);
        await _provider.SalvarAlteracoes();
        return local;
    }
} 