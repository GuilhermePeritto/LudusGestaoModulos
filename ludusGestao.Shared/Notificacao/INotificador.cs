using System.Collections.Generic;

public interface INotificador
{
    void Handle(Notificacao notificacao);
    List<Notificacao> ObterNotificacoes();
    bool TemNotificacao();
} 