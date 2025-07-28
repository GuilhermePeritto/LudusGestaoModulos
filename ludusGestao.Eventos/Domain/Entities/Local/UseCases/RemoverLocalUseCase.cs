using System;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Eventos.Domain.Entities.Local;
using ludusGestao.Eventos.Domain.Entities.Local.Interfaces;

namespace ludusGestao.Eventos.Domain.Entities.Local.UseCases
{
    public class RemoverLocalUseCase : BaseUseCase, IRemoverLocalUseCase
    {
        private readonly ILocalWriteProvider _localWriteProvider;
        private readonly ILocalReadProvider _localReadProvider;

        public RemoverLocalUseCase(
            ILocalWriteProvider localWriteProvider,
            ILocalReadProvider localReadProvider,
            INotificador notificador)
            : base(notificador)
        {
            _localWriteProvider = localWriteProvider;
            _localReadProvider = localReadProvider;
        }

        public async Task<bool> Executar(Guid id)
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
                Notificar("Local não encontrado");
                return false;
            }

            await _localWriteProvider.Remover(id);
            await _localWriteProvider.SalvarAlteracoes();
            
            return true;
        }
    }
} 