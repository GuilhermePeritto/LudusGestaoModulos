using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Usuario;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Gerais.Domain.Usuario.Interfaces
{
    public interface IUsuarioReadProvider : IReadProvider<Usuario>
    {
    }
} 