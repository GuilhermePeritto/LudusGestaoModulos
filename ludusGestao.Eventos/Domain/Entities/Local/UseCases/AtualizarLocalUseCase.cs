using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.Entities.Local.Interfaces;
using LudusGestao.Shared.Notificacao;

namespace ludusGestao.Eventos.Domain.Entities.Local.UseCases
{
    public class AtualizarLocalUseCase : IAtualizarLocalUseCase
    {
        private readonly ILocalWriteProvider _localWriteProvider;
        private readonly INotificador _notificador;

        public AtualizarLocalUseCase(
            ILocalWriteProvider localWriteProvider,
            INotificador notificador)
        {
            _localWriteProvider = localWriteProvider;
            _notificador = notificador;
        }

        public async Task<Local> Executar(Local local)
        {
            await _localWriteProvider.Atualizar(local);
            await _localWriteProvider.SalvarAlteracoes();

            return local;
        }
    }
} 