using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Empresa;

namespace ludusGestao.Gerais.Domain.Empresa.Interfaces
{
    public interface ICriarEmpresaUseCase
    {
        Task<Empresa> Executar(Empresa empresa);
    }
} 