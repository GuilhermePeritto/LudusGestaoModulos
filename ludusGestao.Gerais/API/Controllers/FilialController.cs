using System;
using System.Net;
using LudusGestao.Shared.Domain.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LudusGestao.Shared.Domain.QueryParams;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Gerais.Domain.Filial.DTOs;
using ludusGestao.Gerais.Domain.Filial.Interfaces;

namespace ludusGestao.Gerais.API.Controllers
{
    [ApiController]
    [Route("api/filiais")]
    [Authorize]
    public class FilialController : ControllerRestBase
    {
        private readonly IFilialService _service;

        public FilialController(INotificador notificador, IFilialService service)
            : base(notificador)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] QueryParamsBase query)
        {
            var result = await _service.Listar(query);
            return CustomResponse(HttpStatusCode.OK, result, "Filiais listadas com sucesso.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var result = await _service.BuscarPorId(id);

            if (result == null)
                return CustomResponse(HttpStatusCode.NotFound, $"Filial com código {id} não encontrada");

            return CustomResponse(HttpStatusCode.OK, result, "Filial encontrada com sucesso.");
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarFilialDTO dto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await _service.Criar(dto);

            if (result == null)
                return CustomResponse(HttpStatusCode.BadRequest, "Erro ao criar filial.");

            return CustomResponse(HttpStatusCode.Created, result, "Filial criada com sucesso.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarFilialDTO dto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await _service.Atualizar(id, dto);

            if (result == null)
                return CustomResponse(HttpStatusCode.NotFound, "Filial não encontrada.");

            return CustomResponse(HttpStatusCode.NoContent, result, "Filial atualizada com sucesso.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(Guid id)
        {
            var result = await _service.Remover(id);

            if (!result)
                return CustomResponse(HttpStatusCode.NotFound, "Filial não encontrada.");

            return CustomResponse(HttpStatusCode.NoContent, result, "Filial removida com sucesso.");
        }

        [HttpPatch("{id}/ativar")]
        public async Task<IActionResult> Ativar(Guid id)
        {
            var result = await _service.Ativar(id);

            if (!result)
                return CustomResponse(HttpStatusCode.NotFound, "Filial não encontrada.");

            return CustomResponse(HttpStatusCode.OK, result, "Filial ativada com sucesso.");
        }

        [HttpPatch("{id}/desativar")]
        public async Task<IActionResult> Desativar(Guid id)
        {
            var result = await _service.Desativar(id);

            if (!result)
                return CustomResponse(HttpStatusCode.NotFound, "Filial não encontrada.");

            return CustomResponse(HttpStatusCode.OK, result, "Filial desativada com sucesso.");
        }
    }
} 