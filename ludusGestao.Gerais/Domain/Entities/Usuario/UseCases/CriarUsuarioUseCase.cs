using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using LudusGestao.Shared.Security;
using ludusGestao.Gerais.Domain.Usuario;
using ludusGestao.Gerais.Domain.Usuario.Interfaces;
using ludusGestao.Gerais.Domain.Usuario.Validations;

namespace ludusGestao.Gerais.Domain.Usuario.UseCases
{
    public class CriarUsuarioUseCase : BaseUseCase, ICriarUsuarioUseCase
    {
        private readonly IUsuarioWriteProvider _provider;
        private readonly IPasswordHelper _passwordHelper;

        public CriarUsuarioUseCase(IUsuarioWriteProvider provider, INotificador notificador, IPasswordHelper passwordHelper)
            : base(notificador)
        {
            _provider = provider;
            _passwordHelper = passwordHelper;
        }

        public async Task<Usuario> Executar(Usuario usuario)
        {
            if (!ExecutarValidacao(new CriarUsuarioValidation(), usuario))
                return null;

            // Hash da senha antes de salvar
            usuario.AlterarSenha(_passwordHelper.CriptografarSenha(usuario.Senha));

            await _provider.Adicionar(usuario);
            await _provider.SalvarAlteracoes();
            return usuario;
        }
    }
} 