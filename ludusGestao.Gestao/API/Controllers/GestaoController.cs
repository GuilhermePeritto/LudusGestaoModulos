using System.Net;
using LudusGestao.Shared.Domain.Controllers;
using Microsoft.AspNetCore.Mvc;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Gestao.Domain.Entities.Cliente.DTOs;
using ludusGestao.Gestao.Domain.Entities.Cliente.Interfaces;
using ludusGestao.Provider.Data.Contexts;
using ludusGestao.Provider.Data.Seeds;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using LudusGestao.Shared.Tenant;
using LudusGestao.Shared.Security;

namespace ludusGestao.Gestao.API.Controllers
{
    [ApiController]
    [Route("api/gestao")]
    public class GestaoController : ControllerRestBase
    {
        private readonly ICriarClienteUseCase _criarClienteUseCase;
        private readonly LudusGestaoWriteDbContext _writeContext;
        private readonly ITenantContext _tenantContext;
        private readonly IPasswordHelper _passwordHelper;

        public GestaoController(INotificador notificador, ICriarClienteUseCase criarClienteUseCase, LudusGestaoWriteDbContext writeContext, ITenantContext tenantContext, IPasswordHelper passwordHelper)
            : base(notificador)
        {
            _criarClienteUseCase = criarClienteUseCase;
            _writeContext = writeContext;
            _tenantContext = tenantContext;
            _passwordHelper = passwordHelper;
        }

        /// <summary>
        /// Cria um novo cliente com empresa, filial e usuário administrador
        /// </summary>
        /// <param name="dto">Dados do cliente a ser criado</param>
        /// <returns>Dados do cliente criado incluindo senha do administrador</returns>
        [HttpPost("clientes")]
        public async Task<IActionResult> CriarCliente([FromBody] CriarClienteDTO dto)
        {
            try
            {
                var resultado = await _criarClienteUseCase.Executar(dto);
                
                if (resultado == null)
                    return CustomResponse(HttpStatusCode.BadRequest, null, "Erro ao criar cliente. Verifique os dados informados.");

                return CustomResponse(HttpStatusCode.Created, resultado, "Cliente criado com sucesso!");
            }
            catch (Exception ex)
            {
                return CustomResponse(HttpStatusCode.BadRequest, null, $"Erro ao criar cliente: {ex.Message}");
            }
        }

        /// <summary>
        /// Executa o seed do banco de dados para popular com dados básicos
        /// </summary>
        /// <returns>Resultado da operação</returns>
        [HttpPost("utilitarios/seed")]
        public async Task<IActionResult> ExecutarSeed()
        {
            try
            {
                // Configurar TenantId para o seed
                _tenantContext.SetTenantId(1);
                
                await DatabaseSeeder.SeedAsync(_writeContext, _passwordHelper);
                
                return CustomResponse(HttpStatusCode.OK, null, "Seed executado com sucesso! Dados básicos foram inseridos no banco.");
            }
            catch (Exception ex)
            {
                return CustomResponse(HttpStatusCode.BadRequest, null, $"Erro ao executar seed: {ex.Message}");
            }
        }

        /// <summary>
        /// Verifica o status da conexão com o banco de dados
        /// </summary>
        /// <returns>Status da conexão</returns>
        [HttpGet("utilitarios/health")]
        public async Task<IActionResult> VerificarSaude()
        {
            try
            {
                var podeConectar = await _writeContext.Database.CanConnectAsync();
                
                var mensagem = podeConectar 
                    ? "Conexão com o banco de dados está funcionando normalmente." 
                    : "Não foi possível conectar com o banco de dados.";
                
                return CustomResponse(podeConectar ? HttpStatusCode.OK : HttpStatusCode.ServiceUnavailable, null, mensagem);
            }
            catch (Exception ex)
            {
                return CustomResponse(HttpStatusCode.BadRequest, null, $"Erro ao verificar saúde do banco: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtém informações sobre o banco de dados
        /// </summary>
        /// <returns>Informações do banco</returns>
        [HttpGet("utilitarios/info")]
        public async Task<IActionResult> ObterInformacoes()
        {
            try
            {
                var info = new
                {
                    TipoDeBanco = _writeContext.Database.ProviderName,
                    Empresas = await _writeContext.Empresas.CountAsync(),
                    Filiais = await _writeContext.Filiais.CountAsync(),
                    Usuarios = await _writeContext.Usuarios.CountAsync(),
                    Locais = await _writeContext.Locais.CountAsync(),
                    Status = "Banco conectado com sucesso",
                    Timestamp = DateTime.UtcNow
                };

                return CustomResponse(HttpStatusCode.OK, info, "Informações obtidas com sucesso.");
            }
            catch (Exception ex)
            {
                return CustomResponse(HttpStatusCode.BadRequest, null, $"Erro ao obter informações: {ex.Message}");
            }
        }
    }
} 