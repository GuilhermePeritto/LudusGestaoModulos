using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Gerais.Domain.Usuario;
using ludusGestao.Gerais.Domain.Usuario.Interfaces;
using ludusGestao.Gerais.Domain.Usuario.Validations;

namespace ludusGestao.Gerais.Domain.Usuario.UseCases
{
    public class CriarUsuarioUseCase : BaseUseCase, ICriarUsuarioUseCase
    {
        private readonly IUsuarioWriteProvider _provider;

        public CriarUsuarioUseCase(IUsuarioWriteProvider provider, INotificador notificador)
            : base(notificador)
        {
            _provider = provider;
        }

        public async Task<Usuario> Executar(Usuario usuario)
        {
            if (!ExecutarValidacao(new CriarUsuarioValidation(), usuario))
                return null;

            await _provider.Adicionar(usuario);
            await _provider.SalvarAlteracoes();
            return usuario;
        }
    }
} 