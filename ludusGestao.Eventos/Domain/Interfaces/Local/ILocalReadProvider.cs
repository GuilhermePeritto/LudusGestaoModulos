public interface ILocalReadProvider
{
    Task<Local?> BuscarPorId(Guid id);
    Task<IEnumerable<Local>> ListarTodos();
    Task<IEnumerable<Local>> BuscarPaginado(int pagina, int tamanhoPagina);
}
