using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Eventos.Domain.Local;
using ludusGestao.Eventos.Domain.Local.Interfaces;
using ludusGestao.Eventos.Domain.Local.Validations;

namespace ludusGestao.Eventos.Domain.Local.UseCases
{
    public class AtualizarLocalUseCase : BaseUseCase, IAtualizarLocalUseCase
    {
        private readonly ILocalWriteProvider _writeProvider;
        public AtualizarLocalUseCase(ILocalWriteProvider writeProvider, INotificador notificador)
            : base(notificador)
        {
            _writeProvider = writeProvider;
        }

        public async Task<Local> Executar(Local local)
        {
            if (!ExecutarValidacao(new AtualizarLocalValidation(), local))
                return local;
                
            local.MarcarAlterado();
            await _writeProvider.Atualizar(local);
            await _writeProvider.SalvarAlteracoes();
            return local;
        }
    }
} 