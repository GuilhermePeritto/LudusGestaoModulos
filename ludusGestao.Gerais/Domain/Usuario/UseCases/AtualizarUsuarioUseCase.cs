using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Usuario;
using ludusGestao.Gerais.Domain.Usuario.Interfaces;
using ludusGestao.Gerais.Domain.Usuario.Validations;

namespace ludusGestao.Gerais.Domain.Usuario.UseCases
{
    public class AtualizarUsuarioUseCase : BaseUseCase, IAtualizarUsuarioUseCase
    {
        private readonly IUsuarioWriteProvider _provider;

        public AtualizarUsuarioUseCase(IUsuarioWriteProvider provider, INotificador notificador)
            : base(notificador)
        {
            _provider = provider;
        }

        public async Task<Usuario> Executar(Usuario usuario)
        {
            if (!ExecutarValidacao(new AtualizarUsuarioValidation(), usuario))
                return null;

            await _provider.Atualizar(usuario);
            await _provider.SalvarAlteracoes();
            return usuario;
        }
    }
} 