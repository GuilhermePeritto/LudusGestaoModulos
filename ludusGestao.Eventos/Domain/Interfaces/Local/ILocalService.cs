public interface ILocalService
{
    Task<LocalDTO> Criar(CriarLocalDTO dto);
    Task<LocalDTO> Atualizar(Guid id, AtualizarLocalDTO dto);
    Task<bool> Remover(Guid id);
    Task<LocalDTO> BuscarPorId(Guid id);
    Task<IEnumerable<LocalDTO>> Listar();
} 