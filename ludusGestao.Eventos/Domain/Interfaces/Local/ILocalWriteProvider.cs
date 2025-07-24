public interface ILocalWriteProvider
{
    Task Adicionar(Local local);
    Task Atualizar(Local local);
    Task Remover(Guid id);
    Task<int> SalvarAlteracoes();
}