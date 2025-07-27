using System.Collections.Generic;

namespace LudusGestao.Shared.Notificacao
{
    public interface INotificador
    {
        void Handle(Notificacao notificacao);
        List<Notificacao> ObterNotificacoes();
        bool TemNotificacao();
    }
} 