using System;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Empresa;
using ludusGestao.Gerais.Domain.Empresa.Interfaces;

namespace ludusGestao.Gerais.Domain.Empresa.UseCases
{
    public class RemoverEmpresaUseCase : BaseUseCase, IRemoverEmpresaUseCase
    {
        private readonly IEmpresaWriteProvider _provider;

        public RemoverEmpresaUseCase(IEmpresaWriteProvider provider, INotificador notificador)
            : base(notificador)
        {
            _provider = provider;
        }

        public async Task<bool> Executar(Empresa empresa)
        {
            empresa.Desativar();
            await _provider.Atualizar(empresa);
            await _provider.SalvarAlteracoes();
            return true;
        }
    }
} 