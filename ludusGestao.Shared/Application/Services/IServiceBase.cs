using System.Collections.Generic;
using System.Threading.Tasks;

namespace LudusGestao.Shared.Application.Services
{
    public interface IServiceBase<TCreate, TUpdate, TResponse>
    {
        Task<IEnumerable<TResponse>> Listar();
        Task<TResponse> BuscarPorId(string id);
        Task<TResponse> Criar(TCreate dto);
        Task<TResponse> Atualizar(string id, TUpdate dto);
        Task<bool> Remover(string id);
    }
} 