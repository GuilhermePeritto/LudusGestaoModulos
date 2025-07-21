using LudusGestao.Shared.Application.Controllers;
using ludusGestao.Gerais.Domain.DTOs.Filial;
using ludusGestao.Gerais.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ludusGestao.Gerais.API.Controllers
{
    [ApiController]
    [Route("api/gerais/filiais")]
    public class FilialController : ControllerRestBase<CriarFilialDTO, FilialDTO, AtualizarFilialDTO>
    {
        public FilialController(FilialService service) : base(service) { }
    }
} 