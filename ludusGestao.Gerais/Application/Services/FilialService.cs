using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Filial;
using ludusGestao.Gerais.Application.UseCases.Filial;
using LudusGestao.Shared.Application.Services;

namespace ludusGestao.Gerais.Application.Services
{
    public class FilialService : IServiceBase<CriarFilialDTO, AtualizarFilialDTO, FilialDTO>
    {
        private readonly CriarFilialUseCase _criarUseCase;
        private readonly AtualizarFilialUseCase _atualizarUseCase;
        private readonly RemoverFilialUseCase _removerUseCase;
        private readonly BuscarFilialPorIdUseCase _buscarPorIdUseCase;
        private readonly ListarFiliaisUseCase _listarUseCase;

        public FilialService(
            CriarFilialUseCase criarUseCase,
            AtualizarFilialUseCase atualizarUseCase,
            RemoverFilialUseCase removerUseCase,
            BuscarFilialPorIdUseCase buscarPorIdUseCase,
            ListarFiliaisUseCase listarUseCase)
        {
            _criarUseCase = criarUseCase;
            _atualizarUseCase = atualizarUseCase;
            _removerUseCase = removerUseCase;
            _buscarPorIdUseCase = buscarPorIdUseCase;
            _listarUseCase = listarUseCase;
        }

        public Task<FilialDTO> Criar(CriarFilialDTO dto) => _criarUseCase.Executar(dto);
        public Task<FilialDTO> Atualizar(string id, AtualizarFilialDTO dto) => _atualizarUseCase.Executar(Guid.Parse(id), dto);
        public Task<bool> Remover(string id) => _removerUseCase.Executar(Guid.Parse(id));
        public Task<FilialDTO> BuscarPorId(string id) => _buscarPorIdUseCase.Executar(Guid.Parse(id));
        public Task<IEnumerable<FilialDTO>> Listar() => _listarUseCase.Executar();
    }
} 