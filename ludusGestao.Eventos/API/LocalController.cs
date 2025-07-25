using System.Net;
using LudusGestao.Shared.Application.Controllers;
using Microsoft.AspNetCore.Mvc;
using LudusGestao.Shared.Domain.Common;

[ApiController]
[Route("api/locais")]
public class LocalController : ControllerRestBase
{
    private readonly ILocalService _service;

    public LocalController(INotificador notificador, ILocalService service)
        : base(notificador)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] QueryParamsBase query)
    {
        var result = await _service.Listar(query);
        return CustomResponse(HttpStatusCode.OK, result, "Locais listados com sucesso.");
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var result = await _service.BuscarPorId(id);

        if (result == null)
            return CustomResponse(HttpStatusCode.NotFound, $"Local com codigo {id} n�o encontrado");

        return CustomResponse(HttpStatusCode.OK, result, "Local encontrado com sucesso.");
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarLocalDTO dto)
    {
        if (!ModelState.IsValid)
            return CustomResponse(ModelState);

        var result = await _service.Criar(dto);

        return CustomResponse(HttpStatusCode.Created, result, "Local criado com sucesso.");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarLocalDTO dto)
    {
        if (!ModelState.IsValid)
            return CustomResponse(ModelState);

        var result = await _service.Atualizar(id, dto);

        return CustomResponse(HttpStatusCode.NoContent, result, "Local atualizado com sucesso.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(Guid id)
    {
        var result = await _service.Remover(id);

        return CustomResponse(HttpStatusCode.NoContent, result, "Local removido com sucesso.");
    }
} 