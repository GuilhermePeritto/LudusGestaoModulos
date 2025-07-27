using System.Net;
using LudusGestao.Shared.Domain.Controllers;
using Microsoft.AspNetCore.Mvc;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.DTOs;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Interfaces;

namespace ludusGestao.Autenticacao.API.Controllers
{
    [ApiController]
    [Route("api/autenticacao")]
    public class AutenticacaoController : ControllerRestBase
    {
        private readonly IAutenticacaoService _service;

        public AutenticacaoController(INotificador notificador, IAutenticacaoService service)
            : base(notificador)
        {
            _service = service;
        }

        [HttpPost("entrar")]
        public async Task<IActionResult> Entrar([FromBody] EntrarDTO dto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await _service.Entrar(dto);

            if (result == null)
                return CustomResponse(HttpStatusCode.BadRequest, "Credenciais inválidas.");

            return CustomResponse(HttpStatusCode.OK, result, "Autenticação realizada com sucesso.");
        }

        [HttpPost("atualizar-token")]
        public async Task<IActionResult> AtualizarToken([FromBody] AtualizarTokenDTO dto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await _service.AtualizarToken(dto);

            if (result == null)
                return CustomResponse(HttpStatusCode.BadRequest, "Refresh token inválido ou expirado.");

            return CustomResponse(HttpStatusCode.OK, result, "Token atualizado com sucesso.");
        }
    }
} 