using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Usuario;

namespace ludusGestao.Gerais.Domain.Usuario.Interfaces
{
    public interface IAlterarSenhaUsuarioUseCase
    {
        Task<Usuario> Executar(Usuario usuario, string novaSenha);
    }
} 