using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Filial;
using ludusGestao.Gerais.Domain.Filial.Interfaces;

namespace ludusGestao.Gerais.Domain.Filial.UseCases
{
    public class RemoverFilialUseCase : BaseUseCase, IRemoverFilialUseCase
    {
        private readonly IFilialWriteProvider _provider;

        public RemoverFilialUseCase(IFilialWriteProvider provider, INotificador notificador)
            : base(notificador)
        {
            _provider = provider;
        }

        public async Task<bool> Executar(Filial filial)
        {
            filial.Desativar();
            await _provider.Atualizar(filial);
            await _provider.SalvarAlteracoes();
            return true;
        }
    }
} 