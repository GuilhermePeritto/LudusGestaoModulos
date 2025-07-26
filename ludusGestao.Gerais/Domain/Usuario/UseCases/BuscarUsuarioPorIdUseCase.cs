using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Usuario;
using ludusGestao.Gerais.Domain.Usuario.Interfaces;

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
            var query = new QueryParamsBase 
            { 
                FilterObjects = new List<FilterObject> 
                { 
                    new FilterObject { Property = "Id", Operator = "eq", Value = id } 
                } 
            };
            
            var usuario = await _provider.Buscar(query);
            
            if (usuario == null)
            {
                Notificar("Usuário não encontrado.");
                return null;
            }

            return usuario;
        }
    }
} 