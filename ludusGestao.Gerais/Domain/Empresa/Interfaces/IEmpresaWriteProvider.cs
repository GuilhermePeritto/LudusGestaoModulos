using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Empresa;

namespace ludusGestao.Gerais.Domain.Empresa.Interfaces
{
    public interface IEmpresaWriteProvider
    {
        Task Adicionar(Empresa empresa);
        Task Atualizar(Empresa empresa);
        Task Remover(Guid id);
        Task<int> SalvarAlteracoes();
    }
} 