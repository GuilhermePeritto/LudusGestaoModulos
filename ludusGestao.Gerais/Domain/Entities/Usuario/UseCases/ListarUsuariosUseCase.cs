using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.QueryParams;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Gerais.Domain.Usuario;
using ludusGestao.Gerais.Domain.Usuario.Interfaces;

namespace ludusGestao.Gerais.Domain.Usuario.UseCases
{
    public class ListarUsuariosUseCase : BaseUseCase, IListarUsuariosUseCase
    {
        private readonly IUsuarioReadProvider _provider;

        public ListarUsuariosUseCase(IUsuarioReadProvider provider, INotificador notificador)
            : base(notificador)
        {
            _provider = provider;
        }

        public async Task<IEnumerable<Usuario>> Executar(QueryParamsBase query)
        {
            return await _provider.Listar(query);
        }
    }
} 