using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Usuario;
using ludusGestao.Gerais.Application.UseCases.Usuario;
using LudusGestao.Shared.Application.Services;
using LudusGestao.Shared.Domain.Common;

namespace ludusGestao.Gerais.Application.Services
{
    public class UsuarioService : IServiceBase<CriarUsuarioDTO, AtualizarUsuarioDTO, UsuarioDTO>
    {
        private readonly CriarUsuarioUseCase _criarUseCase;
        private readonly AtualizarUsuarioUseCase _atualizarUseCase;
        private readonly RemoverUsuarioUseCase _removerUseCase;
        private readonly BuscarUsuarioPorIdUseCase _buscarPorIdUseCase;
        private readonly ListarUsuariosUseCase _listarUseCase;

        public UsuarioService(
            CriarUsuarioUseCase criarUseCase,
            AtualizarUsuarioUseCase atualizarUseCase,
            RemoverUsuarioUseCase removerUseCase,
            BuscarUsuarioPorIdUseCase buscarPorIdUseCase,
            ListarUsuariosUseCase listarUseCase)
        {
            _criarUseCase = criarUseCase;
            _atualizarUseCase = atualizarUseCase;
            _removerUseCase = removerUseCase;
            _buscarPorIdUseCase = buscarPorIdUseCase;
            _listarUseCase = listarUseCase;
        }

        public Task<UsuarioDTO> Criar(CriarUsuarioDTO dto) => _criarUseCase.Executar(dto);
        public Task<UsuarioDTO> Atualizar(string id, AtualizarUsuarioDTO dto) => _atualizarUseCase.Executar(Guid.Parse(id), dto);
        public Task<bool> Remover(string id) => _removerUseCase.Executar(Guid.Parse(id));
        public Task<UsuarioDTO> BuscarPorId(string id) => _buscarPorIdUseCase.Executar(Guid.Parse(id));
        public async Task<IEnumerable<UsuarioDTO>> Listar()
        {
            var (itens, _) = await Listar(new QueryParamsBase());
            return itens;
        }

        public async Task<(IEnumerable<UsuarioDTO> Itens, int Total)> Listar(QueryParamsBase query)
        {
            var (itens, total) = await _listarUseCase.ExecutarPaginado(query);
            return (itens, total);
        }
    }
} 