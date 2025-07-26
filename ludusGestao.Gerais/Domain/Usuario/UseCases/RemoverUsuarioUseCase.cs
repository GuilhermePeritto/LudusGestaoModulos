using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Usuario;
using ludusGestao.Gerais.Domain.Usuario.Interfaces;

namespace ludusGestao.Gerais.Domain.Usuario.UseCases
{
    public class RemoverUsuarioUseCase : BaseUseCase, IRemoverUsuarioUseCase
    {
        private readonly IUsuarioWriteProvider _provider;

        public RemoverUsuarioUseCase(IUsuarioWriteProvider provider, INotificador notificador)
            : base(notificador)
        {
            _provider = provider;
        }

        public async Task<bool> Executar(Usuario usuario)
        {
            usuario.Desativar();
            await _provider.Atualizar(usuario);
            await _provider.SalvarAlteracoes();
            return true;
        }
    }
} 