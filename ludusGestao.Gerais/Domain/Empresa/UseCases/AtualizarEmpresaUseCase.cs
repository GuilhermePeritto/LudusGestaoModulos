using ludusGestao.Gerais.Domain.Empresa.Interfaces;
using ludusGestao.Gerais.Domain.Empresa.Validations;

namespace ludusGestao.Gerais.Domain.Empresa.UseCases
{
    public class AtualizarEmpresaUseCase : BaseUseCase, IAtualizarEmpresaUseCase
    {
        private readonly IEmpresaWriteProvider _provider;

        public AtualizarEmpresaUseCase(IEmpresaWriteProvider provider, INotificador notificador)
            : base(notificador)
        {
            _provider = provider;
        }

        public async Task<Empresa> Executar(Empresa empresa)
        {
            if (!ExecutarValidacao(new AtualizarEmpresaValidation(), empresa))
                return null;

            await _provider.Atualizar(empresa);
            await _provider.SalvarAlteracoes();
            return empresa;
        }
    }
} 