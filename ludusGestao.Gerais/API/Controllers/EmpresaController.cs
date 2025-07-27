using System;
using System.Net;
using LudusGestao.Shared.Domain.Controllers;
using Microsoft.AspNetCore.Mvc;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Gerais.Domain.Empresa.DTOs;
using ludusGestao.Gerais.Domain.Empresa.Interfaces;

namespace ludusGestao.Gerais.API.Controllers
{
    [ApiController]
    [Route("api/empresas")]
    public class EmpresaController : ControllerRestBase
    {
        private readonly IEmpresaService _service;

        public EmpresaController(INotificador notificador, IEmpresaService service)
            : base(notificador)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] QueryParamsBase query)
        {
            var result = await _service.Listar(query);
            return CustomResponse(HttpStatusCode.OK, result, "Empresas listadas com sucesso.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var result = await _service.BuscarPorId(id);

            if (result == null)
                return CustomResponse(HttpStatusCode.NotFound, $"Empresa com código {id} não encontrada");

            return CustomResponse(HttpStatusCode.OK, result, "Empresa encontrada com sucesso.");
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarEmpresaDTO dto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await _service.Criar(dto);

            if (result == null)
                return CustomResponse(HttpStatusCode.BadRequest, "Erro ao criar empresa.");

            return CustomResponse(HttpStatusCode.Created, result, "Empresa criada com sucesso.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarEmpresaDTO dto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await _service.Atualizar(id, dto);

            if (result == null)
                return CustomResponse(HttpStatusCode.NotFound, "Empresa não encontrada.");

            return CustomResponse(HttpStatusCode.NoContent, result, "Empresa atualizada com sucesso.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(Guid id)
        {
            var result = await _service.Remover(id);

            if (!result)
                return CustomResponse(HttpStatusCode.NotFound, "Empresa não encontrada.");

            return CustomResponse(HttpStatusCode.NoContent, result, "Empresa removida com sucesso.");
        }

        [HttpPatch("{id}/ativar")]
        public async Task<IActionResult> Ativar(Guid id)
        {
            var result = await _service.Ativar(id);

            if (!result)
                return CustomResponse(HttpStatusCode.NotFound, "Empresa não encontrada.");

            return CustomResponse(HttpStatusCode.OK, result, "Empresa ativada com sucesso.");
        }

        [HttpPatch("{id}/desativar")]
        public async Task<IActionResult> Desativar(Guid id)
        {
            var result = await _service.Desativar(id);

            if (!result)
                return CustomResponse(HttpStatusCode.NotFound, "Empresa não encontrada.");

            return CustomResponse(HttpStatusCode.OK, result, "Empresa desativada com sucesso.");
        }
    }
} 