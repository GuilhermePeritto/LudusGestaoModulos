using System;
using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.Entities;

namespace ludusGestao.Eventos.Domain.Providers
{
    public interface ILocalWriteProvider
    {
        Task Adicionar(Local local);
        Task Atualizar(Local local);
        Task Remover(Guid id);
        Task<int> SalvarAlteracoes();
    }
} 