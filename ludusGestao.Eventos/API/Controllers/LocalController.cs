using ludusGestao.Eventos.Domain.DTOs.Local;
using ludusGestao.Eventos.Application.Service.Local;
using LudusGestao.Shared.Application.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ludusGestao.Eventos.API.Controllers
{
    [ApiController]
    [Route("api/eventos/locais")]
    public class LocalController : ControllerRestBase<CriarLocalDTO, LocalDTO, AtualizarLocalDTO>
    {
        public LocalController(LocalService service) : base(service) { }
    }
} 