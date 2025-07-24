public interface IBuscarLocalPorIdUseCase
{
    Task<Local> Executar(Guid id);
} 