using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.DTOs.Local;
using LudusGestao.Shared.Application.Services;
using ludusGestao.Eventos.Application.UseCases.Local;
using LudusGestao.Shared.Domain.Common;

namespace ludusGestao.Eventos.Application.Service.Local
{
    public class LocalService : IServiceBase<CriarLocalDTO, AtualizarLocalDTO, LocalDTO>
    {
        private readonly CriarLocalUseCase _criarUseCase;
        private readonly AtualizarLocalUseCase _atualizarUseCase;
        private readonly RemoverLocalUseCase _removerUseCase;
        private readonly BuscarLocalPorIdUseCase _buscarPorIdUseCase;
        private readonly ListarLocaisUseCase _listarUseCase;

        public LocalService(
            CriarLocalUseCase criarUseCase,
            AtualizarLocalUseCase atualizarUseCase,
            RemoverLocalUseCase removerUseCase,
            BuscarLocalPorIdUseCase buscarPorIdUseCase,
            ListarLocaisUseCase listarUseCase)
        {
            _criarUseCase = criarUseCase;
            _atualizarUseCase = atualizarUseCase;
            _removerUseCase = removerUseCase;
            _buscarPorIdUseCase = buscarPorIdUseCase;
            _listarUseCase = listarUseCase;
        }

        public Task<LocalDTO> Criar(CriarLocalDTO dto) => _criarUseCase.Executar(dto);
        public Task<LocalDTO> Atualizar(string id, AtualizarLocalDTO dto) => _atualizarUseCase.Executar(id, dto);
        public Task<bool> Remover(string id) => _removerUseCase.Executar(id);
        public Task<LocalDTO> BuscarPorId(string id) => _buscarPorIdUseCase.Executar(id);
        public async Task<(IEnumerable<LocalDTO> Itens, int Total)> Listar(QueryParamsBase query)
            => await _listarUseCase.Executar(query);
    }
} 