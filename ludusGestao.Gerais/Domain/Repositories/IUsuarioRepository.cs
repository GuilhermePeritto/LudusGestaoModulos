using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Entities;
using LudusGestao.Shared.Domain.Common;

namespace ludusGestao.Gerais.Domain.Repositories
{
    public interface IUsuarioRepository
    {
        Task Adicionar(Usuario usuario);
        Task<Usuario> BuscarPorId(Guid id);
        Task Atualizar(Usuario usuario);
        Task Remover(Usuario usuario);
        Task<IEnumerable<Usuario>> ListarTodos();
        Task<bool> ExistePorEmail(string email);
        Task<(IEnumerable<Usuario> Itens, int Total)> ListarPaginado(QueryParamsBase query);
    }
} 