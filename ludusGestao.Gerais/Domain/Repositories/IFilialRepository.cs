using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Entities;

namespace ludusGestao.Gerais.Domain.Repositories
{
    public interface IFilialRepository
    {
        Task Adicionar(Filial filial);
        Task<Filial> BuscarPorId(Guid id);
        Task Atualizar(Filial filial);
        Task Remover(Filial filial);
        Task<IEnumerable<Filial>> ListarTodos();
        Task<bool> ExistePorCodigo(string codigo);
    }
} 