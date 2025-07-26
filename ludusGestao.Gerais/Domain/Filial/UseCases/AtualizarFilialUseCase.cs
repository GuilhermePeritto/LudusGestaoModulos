using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Filial;
using ludusGestao.Gerais.Domain.Filial.Interfaces;
using ludusGestao.Gerais.Domain.Filial.Validations;

namespace ludusGestao.Gerais.Domain.Filial.UseCases
{
    public class AtualizarFilialUseCase : BaseUseCase, IAtualizarFilialUseCase
    {
        private readonly IFilialWriteProvider _provider;

        public AtualizarFilialUseCase(IFilialWriteProvider provider, INotificador notificador)
            : base(notificador)
        {
            _provider = provider;
        }

        public async Task<Filial> Executar(Filial filial)
        {
            if (!ExecutarValidacao(new AtualizarFilialValidation(), filial))
                return null;

            await _provider.Atualizar(filial);
            await _provider.SalvarAlteracoes();
            return filial;
        }
    }
} 