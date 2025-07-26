using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Empresa;

namespace ludusGestao.Gerais.Domain.Empresa.Interfaces
{
    public interface IEmpresaReadProvider
    {
        Task<IEnumerable<Empresa>> Listar();
        Task<IEnumerable<Empresa>> Listar(QueryParamsBase queryParams);
        Task<Empresa> Buscar(QueryParamsBase queryParams);
    }
} 