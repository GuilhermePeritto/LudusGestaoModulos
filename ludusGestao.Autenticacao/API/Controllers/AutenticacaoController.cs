using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ludusGestao.Autenticacao.Application.DTOs;
using ludusGestao.Autenticacao.Application.Services;

namespace ludusGestao.Autenticacao.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly AutenticacaoService _autenticacaoService;

        public AutenticacaoController(AutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [HttpPost("entrar")]
        public async Task<IActionResult> Entrar([FromBody] EntrarDTO dto)
        {
            var (valido, resposta, erro) = await _autenticacaoService.Entrar(dto);
            if (!valido)
                return BadRequest(new { erro });
            return Ok(resposta);
        }

        [HttpPost("atualizar-token")]
        public async Task<IActionResult> AtualizarToken([FromBody] AtualizarTokenDTO dto)
        {
            var (valido, resposta, erro) = await _autenticacaoService.AtualizarToken(dto);
            if (!valido)
                return BadRequest(new { erro });
            return Ok(resposta);
        }
    }
} 