using System;
using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.Entities.Local.Interfaces;
using LudusGestao.Shared.Notificacao;

namespace ludusGestao.Eventos.Domain.Entities.Local.UseCases
{
    public class RemoverLocalUseCase : IRemoverLocalUseCase
    {
        private readonly ILocalWriteProvider _localWriteProvider;
        private readonly ILocalReadProvider _localReadProvider;
        private readonly INotificador _notificador;

        public RemoverLocalUseCase(
            ILocalWriteProvider localWriteProvider,
            ILocalReadProvider localReadProvider,
            INotificador notificador)
        {
            _localWriteProvider = localWriteProvider;
            _localReadProvider = localReadProvider;
            _notificador = notificador;
        }

        public async Task Executar(Guid id)
        {
            var local = await _localReadProvider.Buscar(new LudusGestao.Shared.Domain.Common.QueryParamsBase
            {
                FilterObjects = new System.Collections.Generic.List<LudusGestao.Shared.Domain.Common.FilterObject>
                {
                    new LudusGestao.Shared.Domain.Common.FilterObject
                    {
                        Property = "Id",
                        Operator = "eq",
                        Value = id
                    }
                }
            });

            if (local == null)
            {
                _notificador.Handle(new LudusGestao.Shared.Notificacao.Notificacao("Local não encontrado"));
                return;
            }

            await _localWriteProvider.Remover(id);
            await _localWriteProvider.SalvarAlteracoes();
        }
    }
} 