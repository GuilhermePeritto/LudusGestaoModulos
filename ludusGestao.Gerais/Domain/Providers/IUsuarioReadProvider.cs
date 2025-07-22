using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Entities;

namespace ludusGestao.Gerais.Domain.Providers
{
    public interface IUsuarioReadProvider
    {
        Task<Usuario> BuscarPorId(Guid id);
        Task<IEnumerable<Usuario>> ListarTodos();
        Task<bool> ExistePorEmail(string email);
        Task<(IEnumerable<Usuario> Itens, int Total)> ListarPaginado(LudusGestao.Shared.Domain.Common.QueryParamsBase query);
    }
} 