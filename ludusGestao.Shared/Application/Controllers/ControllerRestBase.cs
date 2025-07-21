using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Application.Services;
using LudusGestao.Shared.Application.Responses;

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
        public virtual async Task<ActionResult<RespostaListaBase<TResponse>>> Listar()
        {
            var lista = await _service.Listar();
            return RespostaPaginada(lista, 1, lista is ICollection<TResponse> col ? col.Count : 0, lista is ICollection<TResponse> col2 ? col2.Count : 0);
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