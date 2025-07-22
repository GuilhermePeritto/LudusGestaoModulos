using ludusGestao.Eventos.Domain.DTOs.Local;
using ludusGestao.Eventos.Application.Service.Local;
using LudusGestao.Shared.Application.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ludusGestao.Eventos.API.Controllers
{
    [ApiController]
    [Route("api/eventos/locais")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class LocalController : ControllerRestBase<CriarLocalDTO, LocalDTO, AtualizarLocalDTO>
    {
        public LocalController(LocalService service, IHttpContextAccessor httpContextAccessor) : base(service) { }
    }
} 