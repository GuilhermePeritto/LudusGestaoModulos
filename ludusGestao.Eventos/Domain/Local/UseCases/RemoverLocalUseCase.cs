using System;
using System.Linq;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Domain.Providers;
using ludusGestao.Eventos.Domain.Local;
using ludusGestao.Eventos.Domain.Local.Interfaces;

namespace ludusGestao.Eventos.Domain.Local.UseCases
{
    public class RemoverLocalUseCase : BaseUseCase
    {
        private readonly ILocalWriteProvider _writeProvider;
        private readonly ILocalReadProvider _readProvider;
        public RemoverLocalUseCase(ILocalWriteProvider writeProvider, ILocalReadProvider readProvider, INotificador notificador)
            : base(notificador)
        {
            _writeProvider = writeProvider;
            _readProvider = readProvider;
        }

        public async Task<bool> Executar(Local local)
        {
            await _writeProvider.Remover(local.Id);
            await _writeProvider.SalvarAlteracoes();
            return true;
        }
    }
} 