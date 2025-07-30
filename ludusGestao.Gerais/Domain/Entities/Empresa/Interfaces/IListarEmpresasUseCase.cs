using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.QueryParams;
using ludusGestao.Gerais.Domain.Empresa.DTOs;

namespace ludusGestao.Gerais.Domain.Empresa.Interfaces
{
    public interface IListarEmpresasUseCase
    {
        Task<IEnumerable<EmpresaDTO>> Executar(QueryParamsBase query);
    }
} 