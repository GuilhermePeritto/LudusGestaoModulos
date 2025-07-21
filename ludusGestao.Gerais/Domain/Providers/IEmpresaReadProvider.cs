using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Entities;

namespace ludusGestao.Gerais.Domain.Providers
{
    public interface IEmpresaReadProvider
    {
        Task<Empresa> BuscarPorId(Guid id);
        Task<IEnumerable<Empresa>> ListarTodos();
        Task<bool> ExistePorCnpj(string cnpj);
    }
} 