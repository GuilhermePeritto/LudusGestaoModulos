using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Filial;

namespace ludusGestao.Gerais.Domain.Filial.Interfaces
{
    public interface IFilialWriteProvider
    {
        Task Adicionar(Filial filial);
        Task Atualizar(Filial filial);
        Task Remover(Guid id);
        Task<int> SalvarAlteracoes();
    }
} 