using LudusGestao.Shared.Application.Controllers;
using ludusGestao.Gerais.Domain.DTOs.Usuario;
using ludusGestao.Gerais.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ludusGestao.Gerais.API.Controllers
{
    [ApiController]
    [Route("api/gerais/usuarios")]
    public class UsuarioController : ControllerRestBase<CriarUsuarioDTO, UsuarioDTO, AtualizarUsuarioDTO>
    {
        public UsuarioController(UsuarioService service) : base(service) { }
    }
} 