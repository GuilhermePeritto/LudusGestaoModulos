using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Filial;

namespace ludusGestao.Gerais.Domain.Filial.Interfaces
{
    public interface IFilialReadProvider
    {
        Task<IEnumerable<Filial>> Listar();
        Task<IEnumerable<Filial>> Listar(QueryParamsBase queryParams);
        Task<Filial> Buscar(QueryParamsBase queryParams);
    }
} 