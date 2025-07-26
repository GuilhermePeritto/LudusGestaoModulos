using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Usuario;

namespace ludusGestao.Gerais.Domain.Usuario.Interfaces
{
    public interface IRemoverUsuarioUseCase
    {
        Task<bool> Executar(Usuario usuario);
    }
} 