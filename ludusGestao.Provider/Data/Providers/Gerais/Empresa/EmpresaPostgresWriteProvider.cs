using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmpresaEntity = ludusGestao.Gerais.Domain.Entities.Empresa;
using ludusGestao.Gerais.Domain.Providers;
using ludusGestao.Provider.Data.Contexts;

namespace ludusGestao.Provider.Data.Providers.Gerais.Empresa
{
    public class EmpresaPostgresWriteProvider : IEmpresaWriteProvider
    {
        private readonly LudusGestaoWriteDbContext _context;
        public EmpresaPostgresWriteProvider(LudusGestaoWriteDbContext context)
        {
            _context = context;
        }

        public async Task Adicionar(EmpresaEntity empresa)
        {
            await _context.Empresas.AddAsync(empresa);
        }

        public async Task Atualizar(EmpresaEntity empresa)
        {
            _context.Empresas.Update(empresa);
        }

        public async Task Remover(Guid id)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa != null)
                _context.Empresas.Remove(empresa);
        }

        public async Task<int> SalvarAlteracoes()
        {
            return await _context.SaveChangesAsync();
        }
    }
} 