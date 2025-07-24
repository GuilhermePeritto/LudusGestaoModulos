public class Notificador : INotificador
{
    private List<Notificacao> _notificacoes = new();
    public void Handle(Notificacao notificacao) => _notificacoes.Add(notificacao);
    public List<Notificacao> ObterNotificacoes() => _notificacoes;
    public bool TemNotificacao() => _notificacoes.Any();
} 