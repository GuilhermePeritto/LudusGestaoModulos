using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Eventos.Domain.Entities.Local;
using ludusGestao.Eventos.Domain.Entities.Local.Interfaces;
using ludusGestao.Eventos.Domain.Entities.Local.Validations;

namespace ludusGestao.Eventos.Domain.Entities.Local.UseCases
{
    public class AtualizarLocalUseCase : BaseUseCase, IAtualizarLocalUseCase
    {
        private readonly ILocalWriteProvider _localWriteProvider;

        public AtualizarLocalUseCase(
            ILocalWriteProvider localWriteProvider,
            INotificador notificador)
            : base(notificador)
        {
            _localWriteProvider = localWriteProvider;
        }

        public async Task<Local> Executar(Local local)
        {
            if (!ExecutarValidacao(new AtualizarLocalValidation(), local))
                return null;

            await _localWriteProvider.Atualizar(local);
            await _localWriteProvider.SalvarAlteracoes();

            return local;
        }
    }
} 