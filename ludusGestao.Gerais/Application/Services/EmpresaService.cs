using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Empresa;
using ludusGestao.Gerais.Application.UseCases.Empresa;
using LudusGestao.Shared.Application.Services;
using LudusGestao.Shared.Domain.Common;

namespace ludusGestao.Gerais.Application.Services
{
    public class EmpresaService : IServiceBase<CriarEmpresaDTO, AtualizarEmpresaDTO, EmpresaDTO>
    {
        private readonly CriarEmpresaUseCase _criarUseCase;
        private readonly AtualizarEmpresaUseCase _atualizarUseCase;
        private readonly RemoverEmpresaUseCase _removerUseCase;
        private readonly BuscarEmpresaPorIdUseCase _buscarPorIdUseCase;
        private readonly ListarEmpresasUseCase _listarUseCase;

        public EmpresaService(
            CriarEmpresaUseCase criarUseCase,
            AtualizarEmpresaUseCase atualizarUseCase,
            RemoverEmpresaUseCase removerUseCase,
            BuscarEmpresaPorIdUseCase buscarPorIdUseCase,
            ListarEmpresasUseCase listarUseCase)
        {
            _criarUseCase = criarUseCase;
            _atualizarUseCase = atualizarUseCase;
            _removerUseCase = removerUseCase;
            _buscarPorIdUseCase = buscarPorIdUseCase;
            _listarUseCase = listarUseCase;
        }

        public Task<EmpresaDTO> Criar(CriarEmpresaDTO dto) => _criarUseCase.Executar(dto);
        public Task<EmpresaDTO> Atualizar(string id, AtualizarEmpresaDTO dto) => _atualizarUseCase.Executar(Guid.Parse(id), dto);
        public Task<bool> Remover(string id) => _removerUseCase.Executar(Guid.Parse(id));
        public Task<EmpresaDTO> BuscarPorId(string id) => _buscarPorIdUseCase.Executar(Guid.Parse(id));
        public async Task<IEnumerable<EmpresaDTO>> Listar()
        {
            var (itens, _) = await Listar(new QueryParamsBase());
            return itens;
        }

        public async Task<(IEnumerable<EmpresaDTO> Itens, int Total)> Listar(QueryParamsBase query)
        {
            var (itens, total) = await _listarUseCase.ExecutarPaginado(query);
            return (itens, total);
        }
    }
} 