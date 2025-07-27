using System;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Gerais.Domain.Usuario;
using ludusGestao.Gerais.Domain.Usuario.Interfaces;
using ludusGestao.Gerais.Domain.Usuario.Validations;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Gerais.Domain.Usuario.UseCases
{
    public class AtualizarUsuarioUseCase : BaseUseCase, IAtualizarUsuarioUseCase
    {
        private readonly IUsuarioWriteProvider _writeProvider;

        public AtualizarUsuarioUseCase(
            IUsuarioWriteProvider writeProvider,
            INotificador notificador)
            : base(notificador)
        {
            _writeProvider = writeProvider;
        }

        public async Task<Usuario> Executar(Usuario usuario)
        {
            if (!ExecutarValidacao(new AtualizarUsuarioValidation(), usuario))
                return null;

            await _writeProvider.Atualizar(usuario);
            await _writeProvider.SalvarAlteracoes();
            return usuario;
        }
    }
} 