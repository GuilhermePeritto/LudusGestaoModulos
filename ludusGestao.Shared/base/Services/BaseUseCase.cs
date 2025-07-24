using FluentValidation;

public abstract class BaseUseCase : BaseService
{
    protected BaseUseCase(INotificador notificador) : base(notificador) { }
} 