using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Gerais.Domain.Usuario;
using ludusGestao.Gerais.Domain.Usuario.Interfaces;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Gerais.Domain.Usuario.UseCases
{
    public class BuscarUsuarioPorIdUseCase : BaseUseCase, IBuscarUsuarioPorIdUseCase
    {
        private readonly IUsuarioReadProvider _provider;

        public BuscarUsuarioPorIdUseCase(IUsuarioReadProvider provider, INotificador notificador)
            : base(notificador)
        {
            _provider = provider;
        }

        public async Task<Usuario> Executar(Guid id)
        {
            var queryParams = QueryParamsHelper.BuscarPorId(id);
            var usuario = await _provider.Buscar(queryParams);
            
            if (usuario == null)
            {
                Notificar("Usuário não encontrado.");
                return null;
            }

            return usuario;
        }
    }
} 