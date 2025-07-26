using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Usuario.DTOs;
using ludusGestao.Gerais.Domain.Usuario.Interfaces;
using ludusGestao.Gerais.Domain.Usuario;

namespace ludusGestao.Gerais.Application.Services
{
    public class UsuarioService : BaseService, IUsuarioService
    {
        private readonly ICriarUsuarioUseCase _criarUseCase;
        private readonly IAtualizarUsuarioUseCase _atualizarUseCase;
        private readonly IRemoverUsuarioUseCase _removerUseCase;
        private readonly IBuscarUsuarioPorIdUseCase _buscarPorIdUseCase;
        private readonly IListarUsuariosUseCase _listarUseCase;

        public UsuarioService(
            ICriarUsuarioUseCase criarUseCase,
            IAtualizarUsuarioUseCase atualizarUseCase,
            IRemoverUsuarioUseCase removerUseCase,
            IBuscarUsuarioPorIdUseCase buscarPorIdUseCase,
            IListarUsuariosUseCase listarUseCase,
            INotificador notificador)
            : base(notificador)
        {
            _criarUseCase = criarUseCase;
            _atualizarUseCase = atualizarUseCase;
            _removerUseCase = removerUseCase;
            _buscarPorIdUseCase = buscarPorIdUseCase;
            _listarUseCase = listarUseCase;
        }

        public async Task<UsuarioDTO> Criar(CriarUsuarioDTO dto)
        {
            var usuario = Usuario.Criar(dto.Nome, dto.Email, dto.Telefone, dto.Cargo, dto.EmpresaId, dto.Senha, dto.Rua, dto.Numero, dto.Bairro, dto.Cidade, dto.Estado, dto.Cep);
            var usuarioCriado = await _criarUseCase.Executar(usuario);
            
            if (usuarioCriado == null)
                return null;

            return UsuarioDTO.Criar(usuarioCriado);
        }

        public async Task<UsuarioDTO> Atualizar(Guid id, AtualizarUsuarioDTO dto)
        {
            var usuario = await _buscarPorIdUseCase.Executar(id);
            
            if (usuario == null)
                return null;

            usuario.Atualizar(dto.Nome, dto.Email, dto.Telefone, dto.Cargo, dto.Rua, dto.Numero, dto.Bairro, dto.Cidade, dto.Estado, dto.Cep);
            var usuarioAtualizado = await _atualizarUseCase.Executar(usuario);
            
            if (usuarioAtualizado == null)
                return null;

            return UsuarioDTO.Criar(usuarioAtualizado);
        }

        public async Task<bool> Remover(Guid id)
        {
            var usuario = await _buscarPorIdUseCase.Executar(id);
            
            if (usuario == null)
                return false;

            return await _removerUseCase.Executar(usuario);
        }

        public async Task<UsuarioDTO> BuscarPorId(Guid id)
        {
            var usuario = await _buscarPorIdUseCase.Executar(id);
            
            if (usuario == null)
                return null;

            return UsuarioDTO.Criar(usuario);
        }

        public async Task<IEnumerable<UsuarioDTO>> Listar(QueryParamsBase query)
        {
            var usuarios = await _listarUseCase.Executar(query);
            return usuarios.Select(UsuarioDTO.Criar);
        }

        public async Task<bool> Ativar(Guid id)
        {
            var usuario = await _buscarPorIdUseCase.Executar(id);
            
            if (usuario == null)
                return false;

            usuario.Ativar();
            var usuarioAtualizado = await _atualizarUseCase.Executar(usuario);
            
            return usuarioAtualizado != null;
        }

        public async Task<bool> Desativar(Guid id)
        {
            var usuario = await _buscarPorIdUseCase.Executar(id);
            
            if (usuario == null)
                return false;

            usuario.Desativar();
            var usuarioAtualizado = await _atualizarUseCase.Executar(usuario);
            
            return usuarioAtualizado != null;
        }

        public async Task<bool> AlterarSenha(Guid id, string novaSenha)
        {
            var usuario = await _buscarPorIdUseCase.Executar(id);
            
            if (usuario == null)
                return false;

            usuario.AlterarSenha(novaSenha);
            var usuarioAtualizado = await _atualizarUseCase.Executar(usuario);
            
            return usuarioAtualizado != null;
        }
    }
} 