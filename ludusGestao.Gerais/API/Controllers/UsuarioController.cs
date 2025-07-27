using System;
using System.Net;
using LudusGestao.Shared.Domain.Controllers;
using Microsoft.AspNetCore.Mvc;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Gerais.Domain.Usuario.DTOs;
using ludusGestao.Gerais.Domain.Usuario.Interfaces;

namespace ludusGestao.Gerais.API.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController : ControllerRestBase
    {
        private readonly IUsuarioService _service;

        public UsuarioController(INotificador notificador, IUsuarioService service)
            : base(notificador)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] QueryParamsBase query)
        {
            var result = await _service.Listar(query);
            return CustomResponse(HttpStatusCode.OK, result, "Usuários listados com sucesso.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var result = await _service.BuscarPorId(id);

            if (result == null)
                return CustomResponse(HttpStatusCode.NotFound, $"Usuário com código {id} não encontrado");

            return CustomResponse(HttpStatusCode.OK, result, "Usuário encontrado com sucesso.");
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarUsuarioDTO dto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await _service.Criar(dto);

            if (result == null)
                return CustomResponse(HttpStatusCode.BadRequest, "Erro ao criar usuário.");

            return CustomResponse(HttpStatusCode.Created, result, "Usuário criado com sucesso.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarUsuarioDTO dto)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var result = await _service.Atualizar(id, dto);

            if (result == null)
                return CustomResponse(HttpStatusCode.NotFound, "Usuário não encontrado.");

            return CustomResponse(HttpStatusCode.NoContent, result, "Usuário atualizado com sucesso.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(Guid id)
        {
            var result = await _service.Remover(id);

            if (!result)
                return CustomResponse(HttpStatusCode.NotFound, "Usuário não encontrado.");

            return CustomResponse(HttpStatusCode.NoContent, result, "Usuário removido com sucesso.");
        }

        [HttpPatch("{id}/ativar")]
        public async Task<IActionResult> Ativar(Guid id)
        {
            var result = await _service.Ativar(id);

            if (!result)
                return CustomResponse(HttpStatusCode.NotFound, "Usuário não encontrado.");

            return CustomResponse(HttpStatusCode.OK, result, "Usuário ativado com sucesso.");
        }

        [HttpPatch("{id}/desativar")]
        public async Task<IActionResult> Desativar(Guid id)
        {
            var result = await _service.Desativar(id);

            if (!result)
                return CustomResponse(HttpStatusCode.NotFound, "Usuário não encontrado.");

            return CustomResponse(HttpStatusCode.OK, result, "Usuário desativado com sucesso.");
        }

        [HttpPatch("{id}/alterar-senha")]
        public async Task<IActionResult> AlterarSenha(Guid id, [FromBody] string novaSenha)
        {
            if (string.IsNullOrEmpty(novaSenha))
                return CustomResponse(HttpStatusCode.BadRequest, "Nova senha é obrigatória.");

            var result = await _service.AlterarSenha(id, novaSenha);

            if (!result)
                return CustomResponse(HttpStatusCode.NotFound, "Usuário não encontrado.");

            return CustomResponse(HttpStatusCode.OK, result, "Senha alterada com sucesso.");
        }
    }
} 