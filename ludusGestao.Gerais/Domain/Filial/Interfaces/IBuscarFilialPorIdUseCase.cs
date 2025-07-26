using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Filial;

namespace ludusGestao.Gerais.Domain.Filial.Interfaces
{
    public interface IBuscarFilialPorIdUseCase
    {
        Task<Filial> Executar(Guid id);
    }
} 