using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Domain.Responses;
using LudusGestao.Shared.Notificacao;

namespace LudusGestao.Shared.Domain.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ControllerRestBase : ControllerBase
    {
        protected readonly INotificador _notificador;

        protected ControllerRestBase(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected IActionResult Resposta<T>(T data, string message = null, bool success = true)
        {
            var resposta = new RespostaBase(data, message, _notificador.ObterNotificacoes().Select(n => n.Mensagem).ToList());
            return Ok(resposta);
        }

        protected IActionResult Resposta(string message, bool success = true)
        {
            var resposta = new RespostaBase(null, message, _notificador.ObterNotificacoes().Select(n => n.Mensagem).ToList());
            return Ok(resposta);
        }

        protected IActionResult CustomResponse(HttpStatusCode statusCode, object data = null, string message = null)
        {
            var resposta = new RespostaBase(data, message, _notificador.ObterNotificacoes().Select(n => n.Mensagem).ToList());
            return StatusCode((int)statusCode, resposta);
        }

        protected IActionResult CustomResponse(object data, string message = null)
        {
            return CustomResponse(HttpStatusCode.OK, data, message);
        }

        protected IActionResult CustomResponse(string message)
        {
            return CustomResponse(HttpStatusCode.OK, null, message);
        }

        protected IActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            var resposta = new RespostaBase(null, "Dados inválidos", erros);
            return BadRequest(resposta);
        }
    }
} 