using System.Collections.Generic;
using System.Linq;
using LudusGestao.Shared.Notificacao;

namespace LudusGestao.Shared.Domain.Common
{
    public abstract class BaseService
    {
        protected readonly INotificador _notificador;

        protected BaseService(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected void Notificar(string mensagem)
        {
            _notificador.Handle(new LudusGestao.Shared.Notificacao.Notificacao(mensagem));
        }

        protected void Notificar(List<string> mensagens)
        {
            foreach (var mensagem in mensagens)
            {
                Notificar(mensagem);
            }
        }
    }
} 