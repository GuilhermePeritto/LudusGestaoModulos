public interface IListarLocaisUseCase
{
    Task<IEnumerable<Local>> Executar();
} 