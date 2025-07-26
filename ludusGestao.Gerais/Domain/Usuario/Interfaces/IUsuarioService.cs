using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Usuario.DTOs;

namespace ludusGestao.Gerais.Domain.Usuario.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioDTO> Criar(CriarUsuarioDTO dto);
        Task<UsuarioDTO> Atualizar(Guid id, AtualizarUsuarioDTO dto);
        Task<bool> Remover(Guid id);
        Task<UsuarioDTO> BuscarPorId(Guid id);
        Task<IEnumerable<UsuarioDTO>> Listar(QueryParamsBase query);
        Task<bool> Ativar(Guid id);
        Task<bool> Desativar(Guid id);
        Task<bool> AlterarSenha(Guid id, string novaSenha);
    }
} 