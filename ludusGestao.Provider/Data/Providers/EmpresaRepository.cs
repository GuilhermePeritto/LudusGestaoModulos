using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Entities;
using ludusGestao.Gerais.Domain.Repositories;

namespace ludusGestao.Provider.Data.Providers
{
    public class EmpresaRepository : IEmpresaRepository, LudusGestao.Shared.Domain.Repositories.IRepository<Empresa>
    {
        private readonly ludusGestao.Provider.Data.Contexts.LudusGestaoReadDbContext _readContext;
        private readonly ludusGestao.Provider.Data.Contexts.LudusGestaoWriteDbContext _writeContext;

        public EmpresaRepository(
            ludusGestao.Provider.Data.Contexts.LudusGestaoReadDbContext readContext,
            ludusGestao.Provider.Data.Contexts.LudusGestaoWriteDbContext writeContext)
        {
            _readContext = readContext;
            _writeContext = writeContext;
        }

        public async Task Adicionar(Empresa empresa)
        {
            await _writeContext.Empresas.AddAsync(empresa);
            await _writeContext.SaveChangesAsync();
        }

        public async Task<Empresa> BuscarPorId(Guid id)
        {
            return await _readContext.Empresas.FindAsync(id);
        }

        public async Task<Empresa> BuscarPorId(string id)
        {
            if (!Guid.TryParse(id, out var guid))
                return null;
            return await BuscarPorId(guid);
        }

        public async Task Atualizar(Empresa empresa)
        {
            _writeContext.Empresas.Update(empresa);
            await _writeContext.SaveChangesAsync();
        }

        public async Task Remover(Empresa empresa)
        {
            _writeContext.Empresas.Remove(empresa);
            await _writeContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Empresa>> ListarTodos()
        {
            return await System.Threading.Tasks.Task.FromResult(_readContext.Empresas);
        }

        public async Task<bool> ExistePorCnpj(string cnpj)
        {
            return await System.Threading.Tasks.Task.FromResult(_readContext.Empresas.Any(e => e.Cnpj.Valor == cnpj));
        }
    }
} 