using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.QueryParams;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Gerais.Domain.Filial;
using ludusGestao.Gerais.Domain.Filial.Interfaces;
using ludusGestao.Gerais.Domain.Filial.DTOs;

namespace ludusGestao.Gerais.Domain.Filial.UseCases
{
    public class ListarFiliaisUseCase : BaseUseCase, IListarFiliaisUseCase
    {
        private readonly IFilialReadProvider _provider;

        public ListarFiliaisUseCase(IFilialReadProvider provider, INotificador notificador)
            : base(notificador)
        {
            _provider = provider;
        }

        public async Task<IEnumerable<FilialDTO>> Executar(QueryParamsBase query)
        {
            var filiais = await _provider.Listar(query);
            return filiais.Cast<Filial>().Select(FilialDTO.Criar);
        }
    }
} 
