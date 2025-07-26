using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Filial;

namespace ludusGestao.Gerais.Domain.Filial.Interfaces
{
    public interface IRemoverFilialUseCase
    {
        Task<bool> Executar(Filial filial);
    }
} 