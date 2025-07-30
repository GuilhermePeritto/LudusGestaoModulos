using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.QueryParams;
using ludusGestao.Gerais.Domain.Filial.DTOs;

namespace ludusGestao.Gerais.Domain.Filial.Interfaces
{
    public interface IListarFiliaisUseCase
    {
        Task<IEnumerable<FilialDTO>> Executar(QueryParamsBase query);
    }
} 