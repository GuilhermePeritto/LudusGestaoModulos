using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Empresa;

namespace ludusGestao.Gerais.Domain.Empresa.Interfaces
{
    public interface IListarEmpresasUseCase
    {
        Task<IEnumerable<Empresa>> Executar(QueryParamsBase query);
    }
} 