using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Usuario;

namespace ludusGestao.Gerais.Domain.Usuario.Interfaces
{
    public interface IBuscarUsuarioPorIdUseCase
    {
        Task<Usuario> Executar(Guid id);
    }
} 