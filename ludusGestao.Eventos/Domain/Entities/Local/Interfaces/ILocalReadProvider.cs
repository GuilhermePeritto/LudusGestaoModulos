using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Eventos.Domain.Local.Interfaces
{
    public interface ILocalReadProvider : IReadProvider<ludusGestao.Eventos.Domain.Local.Local>
    {
    }
}
