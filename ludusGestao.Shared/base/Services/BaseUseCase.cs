using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using LudusGestao.Shared.Notificacao;

namespace LudusGestao.Shared.Domain.Common
{
    public abstract class BaseUseCase
    {
        protected readonly INotificador _notificador;

        protected BaseUseCase(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : class
        {
            var validator = validacao.Validate(entidade);

            if (validator.IsValid) return true;

            foreach (var error in validator.Errors)
            {
                _notificador.Handle(new LudusGestao.Shared.Notificacao.Notificacao(error.ErrorMessage));
            }

            return false;
        }

        protected void Notificar(string mensagem)
        {
            _notificador.Handle(new LudusGestao.Shared.Notificacao.Notificacao(mensagem));
        }
    }
} 