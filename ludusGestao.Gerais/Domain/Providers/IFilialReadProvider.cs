using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Entities;

namespace ludusGestao.Gerais.Domain.Providers
{
    public interface IFilialReadProvider
    {
        Task<Filial> BuscarPorId(Guid id);
        Task<IEnumerable<Filial>> ListarTodos();
        Task<bool> ExistePorCodigo(string codigo);
        Task<(IEnumerable<Filial> Itens, int Total)> ListarPaginado(LudusGestao.Shared.Domain.Common.QueryParamsBase query);
    }
} 