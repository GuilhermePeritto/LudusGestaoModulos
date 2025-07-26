using LudusGestao.Shared.Domain.Common;
using ludusGestao.Eventos.Domain.Local.DTOs;
using ludusGestao.Eventos.Domain.Local.Interfaces;
using ludusGestao.Eventos.Domain.Local;


namespace ludusGestao.Eventos.Application.Services
{
    public class LocalService : BaseService, ILocalService
    {
        private readonly ICriarLocalUseCase _criarUseCase;
        private readonly IAtualizarLocalUseCase _atualizarUseCase;
        private readonly IRemoverLocalUseCase _removerUseCase;
        private readonly IBuscarLocalPorIdUseCase _buscarPorIdUseCase;
        private readonly IListarLocaisUseCase _listarUseCase;
        public LocalService(
            ICriarLocalUseCase criarUseCase,
            IAtualizarLocalUseCase atualizarUseCase,
            IRemoverLocalUseCase removerUseCase,
            IBuscarLocalPorIdUseCase buscarPorIdUseCase,
            IListarLocaisUseCase listarUseCase,
            INotificador notificador)
            : base(notificador)
        {
            _criarUseCase = criarUseCase;
            _atualizarUseCase = atualizarUseCase;
            _removerUseCase = removerUseCase;
            _buscarPorIdUseCase = buscarPorIdUseCase;
            _listarUseCase = listarUseCase;
        }

        public async Task<LocalDTO> Criar(CriarLocalDTO dto)
        {
            var local = Local.Criar(dto.Nome, dto.Rua, dto.Numero, dto.Bairro, dto.Cidade, dto.Estado, dto.Cep, dto.Capacidade);
            var localCriado = await _criarUseCase.Executar(local);
            return LocalDTO.Criar(localCriado);
        }

        public async Task<LocalDTO> Atualizar(Guid id, AtualizarLocalDTO dto)
        {
            var local = await _buscarPorIdUseCase.Executar(id);
            local.Atualizar(dto.Nome, dto.Rua, dto.Numero, dto.Bairro, dto.Cidade, dto.Estado, dto.Cep, dto.Capacidade);
            await _atualizarUseCase.Executar(local);
            return LocalDTO.Criar(local);
        }

        public async Task<bool> Remover(Guid id)
        {
            var local = await _buscarPorIdUseCase.Executar(id);
            return await _removerUseCase.Executar(local);
        }

        public async Task<LocalDTO> BuscarPorId(Guid id)
        {
            var local = await _buscarPorIdUseCase.Executar(id);
            return LocalDTO.Criar(local);
        }

        public async Task<IEnumerable<LocalDTO>> Listar(QueryParamsBase query)
        {
            var locais = await _listarUseCase.Executar(query);
            return locais.Select(LocalDTO.Criar);
        }
    }
} 