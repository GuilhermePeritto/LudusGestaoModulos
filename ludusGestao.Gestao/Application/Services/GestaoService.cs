using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Gestao.Domain.Entities.Cliente.DTOs;
using ludusGestao.Gestao.Domain.Entities.Cliente.Interfaces;
using ludusGestao.Gestao.Application.Services.Interfaces;

namespace ludusGestao.Gestao.Application.Services
{
    public class GestaoService : BaseService, IGestaoService
    {
        private readonly ICriarClienteUseCase _criarClienteUseCase;

        public GestaoService(
            ICriarClienteUseCase criarClienteUseCase,
            INotificador notificador)
            : base(notificador)
        {
            _criarClienteUseCase = criarClienteUseCase;
        }

        public async Task<ClienteCriadoDTO> CriarCliente(CriarClienteDTO dto)
        {
            var result = await _criarClienteUseCase.Executar(dto);
            
            if (result == null)
                return null;

            return result;
        }
    }
} 