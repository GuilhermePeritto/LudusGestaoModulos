using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Entities;

namespace ludusGestao.Gerais.Domain.Providers
{
    public interface IFilialWriteProvider
    {
        Task Adicionar(Filial filial);
        Task Atualizar(Filial filial);
        Task Remover(Guid id);
        Task<int> SalvarAlteracoes();
    }
} 