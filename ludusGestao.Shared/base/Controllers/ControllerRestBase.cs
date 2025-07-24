using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using LudusGestao.Shared.Application.Responses;

namespace LudusGestao.Shared.Application.Controllers
{
    [ApiController]
    public abstract class ControllerRestBase : ControllerBase
    {
        private readonly INotificador _notificador;

        protected ControllerRestBase(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }

        protected ActionResult CustomResponse(HttpStatusCode statusCode = HttpStatusCode.OK, object result = null, string mensagem = null, int? totalItens = null, int? paginaAtual = null, int? tamanhoPagina = null)
        {
            if (OperacaoValida())
            {
                var resposta = new RespostaBase(result, mensagem, null, totalItens, paginaAtual, tamanhoPagina);
                return new ObjectResult(resposta) { StatusCode = (int)statusCode };
            }

            var respostaErro = new RespostaBase(null, mensagem, _notificador.ObterNotificacoes().Select(n => n.Mensagem).ToList());
            return BadRequest(respostaErro);
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
                NotificarModelInvalida(modelState);

            return CustomResponse();
        }

        protected void NotificarModelInvalida(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(p => p.Errors);
            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception is null ? erro.ErrorMessage : erro.Exception.Message;
                NotificarErro(errorMsg);
            }
        }

        protected void NotificarErro(string erro)
        {
            _notificador.Handle(new Notificacao(erro));
        }
    }
} 