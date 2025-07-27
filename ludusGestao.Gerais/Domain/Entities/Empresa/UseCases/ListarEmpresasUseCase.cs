using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Gerais.Domain.Empresa;
using ludusGestao.Gerais.Domain.Empresa.Interfaces;

namespace ludusGestao.Gerais.Domain.Empresa.UseCases
{
    public class ListarEmpresasUseCase : BaseUseCase, IListarEmpresasUseCase
    {
        private readonly IEmpresaReadProvider _provider;

        public ListarEmpresasUseCase(IEmpresaReadProvider provider, INotificador notificador)
            : base(notificador)
        {
            _provider = provider;
        }

        public async Task<IEnumerable<Empresa>> Executar(QueryParamsBase query)
        {
            return await _provider.Listar(query);
        }
    }
} 
