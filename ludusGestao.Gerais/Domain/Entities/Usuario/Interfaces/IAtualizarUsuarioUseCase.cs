using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Usuario;

namespace ludusGestao.Gerais.Domain.Usuario.Interfaces
{
    public interface IAtualizarUsuarioUseCase
    {
        Task<Usuario> Executar(Usuario usuario);
    }
} 