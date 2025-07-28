using System;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using LudusGestao.Shared.Security;
using ludusGestao.Gerais.Domain.Usuario;
using ludusGestao.Gerais.Domain.Usuario.Interfaces;
using ludusGestao.Gerais.Domain.Usuario.Validations;

namespace ludusGestao.Gerais.Domain.Usuario.UseCases
{
    public class AlterarSenhaUsuarioUseCase : BaseUseCase, IAlterarSenhaUsuarioUseCase
    {
        private readonly IUsuarioWriteProvider _writeProvider;
        private readonly IPasswordHelper _passwordHelper;

        public AlterarSenhaUsuarioUseCase(
            IUsuarioWriteProvider writeProvider,
            IPasswordHelper passwordHelper,
            INotificador notificador)
            : base(notificador)
        {
            _writeProvider = writeProvider;
            _passwordHelper = passwordHelper;
        }

        public async Task<Usuario> Executar(Usuario usuario, string novaSenha)
        {
            if (!ExecutarValidacao(new AlterarSenhaUsuarioValidation(), novaSenha))
                return null;

            var senhaCriptografada = _passwordHelper.CriptografarSenha(novaSenha);
            usuario.AlterarSenha(senhaCriptografada);

            await _writeProvider.Atualizar(usuario);
            await _writeProvider.SalvarAlteracoes();
            
            return usuario;
        }
    }
} 