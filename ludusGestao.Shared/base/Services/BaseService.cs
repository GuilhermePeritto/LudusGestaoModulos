using FluentValidation;

public abstract class BaseService
{
    private readonly INotificador _notificador;
    protected BaseService(INotificador notificador) => _notificador = notificador;

    protected void Notificar(string mensagem) => _notificador.Handle(new Notificacao(mensagem));

    protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE>
    {
        var validator = validacao.Validate(entidade);
        if (validator.IsValid) return true;
        foreach (var error in validator.Errors)
            Notificar(error.ErrorMessage);
        return false;
    }
} 