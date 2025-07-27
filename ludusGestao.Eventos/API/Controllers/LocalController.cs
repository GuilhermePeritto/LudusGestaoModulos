using Microsoft.AspNetCore.Mvc;
using LudusGestao.Shared.Domain.Controllers;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Eventos.Domain.Entities.Local.Interfaces;
using ludusGestao.Eventos.Domain.Entities.Local.DTOs;
using ludusGestao.Eventos.Application.Services;

namespace ludusGestao.Eventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocalController : ControllerRestBase
    {
        private readonly ILocalService _localService;

        public LocalController(ILocalService localService, INotificador notificador) : base(notificador)
        {
            _localService = localService;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarLocalDTO dto)
        {
            var local = await _localService.Criar(dto);
            return Resposta(local, "Local criado com sucesso");
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var locais = await _localService.Listar();
            return Resposta(locais, "Locais listados com sucesso");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            var local = await _localService.BuscarPorId(id);
            return Resposta(local, "Local encontrado com sucesso");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarLocalDTO dto)
        {
            var local = await _localService.Atualizar(id, dto);
            return Resposta(local, "Local atualizado com sucesso");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(Guid id)
        {
            await _localService.Remover(id);
            return Resposta("Local removido com sucesso", true);
        }
    }
} 