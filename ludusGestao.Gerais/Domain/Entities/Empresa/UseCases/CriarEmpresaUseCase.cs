using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Gerais.Domain.Empresa;
using ludusGestao.Gerais.Domain.Empresa.Interfaces;
using ludusGestao.Gerais.Domain.Empresa.Validations;

namespace ludusGestao.Gerais.Domain.Empresa.UseCases
{
    public class CriarEmpresaUseCase : BaseUseCase, ICriarEmpresaUseCase
    {
        private readonly IEmpresaWriteProvider _provider;

        public CriarEmpresaUseCase(IEmpresaWriteProvider provider, INotificador notificador)
            : base(notificador)
        {
            _provider = provider;
        }

        public async Task<Empresa> Executar(Empresa empresa)
        {
            if (!ExecutarValidacao(new CriarEmpresaValidation(), empresa))
                return null;

            await _provider.Adicionar(empresa);
            await _provider.SalvarAlteracoes();
            return empresa;
        }
    }
} 
