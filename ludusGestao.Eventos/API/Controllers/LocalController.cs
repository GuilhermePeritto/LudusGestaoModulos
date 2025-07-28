using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LudusGestao.Shared.Domain.Controllers;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Eventos.Domain.Entities.Local.Interfaces;
using ludusGestao.Eventos.Domain.Entities.Local.DTOs;
using ludusGestao.Eventos.Application.Services;

namespace ludusGestao.Eventos.API.Controllers
{
    [ApiController]
    [Route("api/locais")]
    [Authorize]
    public class LocalController : ControllerRestBase
    {
        private readonly ILocalService _localService;

        public LocalController(ILocalService localService, INotificador notificador) 
            : base(notificador)
        {
            _localService = localService;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarLocalDTO dto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await _localService.Criar(dto);

            if (result == null)
                return CustomResponse(HttpStatusCode.BadRequest, "Erro ao criar local.");

            return CustomResponse(HttpStatusCode.Created, result, "Local criado com sucesso.");
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var result = await _localService.Listar();
            return CustomResponse(HttpStatusCode.OK, result, "Locais listados com sucesso.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var result = await _localService.BuscarPorId(id);

            if (result == null)
                return CustomResponse(HttpStatusCode.NotFound, $"Local com código {id} não encontrado");

            return CustomResponse(HttpStatusCode.OK, result, "Local encontrado com sucesso.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarLocalDTO dto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await _localService.Atualizar(id, dto);

            if (result == null)
                return CustomResponse(HttpStatusCode.NotFound, "Local não encontrado.");

            return CustomResponse(HttpStatusCode.NoContent, result, "Local atualizado com sucesso.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(Guid id)
        {
            var result = await _localService.Remover(id);

            if (!result)
                return CustomResponse(HttpStatusCode.NotFound, "Local não encontrado.");

            return CustomResponse(HttpStatusCode.NoContent, result, "Local removido com sucesso.");
        }
    }
} 