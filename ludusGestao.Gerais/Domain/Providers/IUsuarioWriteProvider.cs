using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Entities;

namespace ludusGestao.Gerais.Domain.Providers
{
    public interface IUsuarioWriteProvider
    {
        Task Adicionar(Usuario usuario);
        Task Atualizar(Usuario usuario);
        Task Remover(Guid id);
        Task<int> SalvarAlteracoes();
    }
} 