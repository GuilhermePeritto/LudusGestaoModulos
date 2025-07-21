using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Entities;
using LudusGestao.Shared.Domain.Common;

namespace ludusGestao.Gerais.Domain.Repositories
{
    public interface IEmpresaRepository
    {
        Task Adicionar(Empresa empresa);
        Task<Empresa> BuscarPorId(Guid id);
        Task Atualizar(Empresa empresa);
        Task Remover(Empresa empresa);
        Task<IEnumerable<Empresa>> ListarTodos();
        Task<bool> ExistePorCnpj(string cnpj);
        Task<(IEnumerable<Empresa> Itens, int Total)> ListarPaginado(QueryParamsBase query);
    }
} 