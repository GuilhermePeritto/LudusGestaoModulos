using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Filial;

namespace ludusGestao.Gerais.Domain.Filial.Interfaces
{
    public interface IListarFiliaisUseCase
    {
        Task<IEnumerable<Filial>> Executar(QueryParamsBase query);
    }
} 