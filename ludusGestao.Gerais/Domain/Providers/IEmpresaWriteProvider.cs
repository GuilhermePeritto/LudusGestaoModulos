using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Entities;

namespace ludusGestao.Gerais.Domain.Providers
{
    public interface IEmpresaWriteProvider
    {
        Task Adicionar(Empresa empresa);
        Task Atualizar(Empresa empresa);
        Task Remover(Guid id);
        Task<int> SalvarAlteracoes();
    }
} 