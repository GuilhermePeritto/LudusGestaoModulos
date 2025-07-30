using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Eventos.Domain.Local;
using ludusGestao.Eventos.Domain.Local.Interfaces;
using ludusGestao.Eventos.Domain.Local.DTOs;

namespace ludusGestao.Eventos.Domain.Local.UseCases
{
    public class ListarLocaisUseCase : BaseUseCase, IListarLocaisUseCase
    {
        private readonly ILocalReadProvider _localReadProvider;

        public ListarLocaisUseCase(ILocalReadProvider localReadProvider, INotificador notificador)
            : base(notificador)
        {
            _localReadProvider = localReadProvider;
        }

        public async Task<IEnumerable<LocalDTO>> Executar()
        {
            var locais = await _localReadProvider.Listar();
            return locais.Select(LocalDTO.Criar);
        }
    }
} 