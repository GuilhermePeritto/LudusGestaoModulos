using System;
using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.Local;

namespace ludusGestao.Eventos.Domain.Local.Interfaces
{
    public interface ILocalWriteProvider
    {
        Task Adicionar(Local local);
        Task Atualizar(Local local);
        Task Remover(Guid id);
        Task<int> SalvarAlteracoes();
    }
}