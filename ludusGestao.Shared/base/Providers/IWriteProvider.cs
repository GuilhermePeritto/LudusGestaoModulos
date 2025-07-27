using System;
using System.Threading.Tasks;

namespace LudusGestao.Shared.Domain.Providers
{
    public interface IWriteProvider<TEntity> where TEntity : class
    {
        Task Adicionar(TEntity entity);
        Task Atualizar(TEntity entity);
        Task Remover(Guid id);
        Task<int> SalvarAlteracoes();
    }
} 