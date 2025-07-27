using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Empresa;

namespace ludusGestao.Gerais.Domain.Empresa.Interfaces
{
    public interface IBuscarEmpresaPorIdUseCase
    {
        Task<Empresa> Executar(Guid id);
    }
} 