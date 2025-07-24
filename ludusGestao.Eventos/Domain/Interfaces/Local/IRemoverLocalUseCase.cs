public interface IRemoverLocalUseCase
{
    Task<bool> Executar(Local local);
} 