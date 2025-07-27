using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Empresa;

namespace ludusGestao.Gerais.Domain.Empresa.Interfaces
{
    public interface IRemoverEmpresaUseCase
    {
        Task<bool> Executar(Empresa empresa);
    }
} 