using LudusGestao.Shared.Application.Controllers;
using ludusGestao.Gerais.Domain.DTOs.Empresa;
using ludusGestao.Gerais.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ludusGestao.Gerais.API.Controllers
{
    [ApiController]
    [Route("api/gerais/empresas")]
    public class EmpresaController : ControllerRestBase<CriarEmpresaDTO, EmpresaDTO, AtualizarEmpresaDTO>
    {
        public EmpresaController(EmpresaService service) : base(service) { }
    }
} 