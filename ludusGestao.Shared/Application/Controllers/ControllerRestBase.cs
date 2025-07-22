using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Application.Services;
using LudusGestao.Shared.Application.Responses;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Application.Events;
using Microsoft.AspNetCore.Http;

namespace LudusGestao.Shared.Application.Controllers
{
    [ApiController]
    public abstract class ControllerRestBase<TCreate, TResponse, TUpdate> : ControllerBase
        where TCreate : class
        where TResponse : class
        where TUpdate : class
    {
        protected readonly IServiceBase<TCreate, TUpdate, TResponse> _service;

        protected ControllerRestBase(IServiceBase<TCreate, TUpdate, TResponse> service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<RespostaListaBase<TResponse>>> Listar([FromQuery] QueryParamsBase query)
        {
            // Validação customizada dos parâmetros de paginação
            if (query.Page <= 0 || query.Limit <= 0)
            {
                var httpContextAccessor = HttpContext?.RequestServices.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
                if (httpContextAccessor != null)
                {
                    await httpContextAccessor.PublicarErro(new ErroEvento {
                        Codigo = "VALIDACAO",
                        Mensagem = "Parâmetros de paginação inválidos.",
                        Erros = new List<string> { "Page deve ser maior que 0.", "Limit deve ser maior que 0." },
                        StatusCode = 400
                    });
                }
                var respostaErro = new RespostaBase<object>(null)
                {
                    Sucesso = false,
                    Mensagem = "Parâmetros de paginação inválidos.",
                    Erros = new List<string> { "Page deve ser maior que 0.", "Limit deve ser maior que 0." }
                };
                return BadRequest(respostaErro);
            }
            var (itens, total) = await _service.Listar(query);
            return RespostaPaginada(itens, query.Page, query.Limit, total);
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<RespostaBase<TResponse>>> BuscarPorId([FromRoute] string id)
        {
            var item = await _service.BuscarPorId(id);
            return RespostaSucesso(item);
        }

        [HttpPost]
        public virtual async Task<ActionResult<RespostaBase<TResponse>>> Criar([FromBody] TCreate dto)
        {
            var item = await _service.Criar(dto);
            return RespostaSucesso(item, "Criado com sucesso.");
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<RespostaBase<TResponse>>> Atualizar([FromRoute] string id, [FromBody] TUpdate dto)
        {
            var item = await _service.Atualizar(id, dto);
            return RespostaSucesso(item, "Atualizado com sucesso.");
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult<RespostaBase<bool>>> Remover([FromRoute] string id)
        {
            var sucesso = await _service.Remover(id);
            return RespostaSucesso(sucesso, "Removido com sucesso.");
        }

        protected ActionResult<RespostaBase<T>> RespostaSucesso<T>(T conteudo, string mensagem = null)
        {
            return Ok(new RespostaBase<T>(conteudo, mensagem));
        }

        protected ActionResult<RespostaListaBase<T>> RespostaPaginada<T>(IEnumerable<T> itens, int paginaAtual, int tamanhoPagina, int totalItens, string mensagem = null)
        {
            return Ok(new RespostaListaBase<T>(itens, paginaAtual, tamanhoPagina, totalItens, mensagem));
        }
    }
} 