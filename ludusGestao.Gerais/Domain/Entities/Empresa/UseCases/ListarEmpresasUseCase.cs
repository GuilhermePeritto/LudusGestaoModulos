using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.QueryParams;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Gerais.Domain.Empresa;
using ludusGestao.Gerais.Domain.Empresa.Interfaces;
using ludusGestao.Gerais.Domain.Empresa.DTOs;

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

        public async Task<IEnumerable<EmpresaDTO>> Executar(QueryParamsBase query)
        {
            // ✅ UseCase limpo: sempre usa o mesmo fluxo
            // O provider se encarrega de verificar fields e retornar o que foi pedido
            var empresas = await _provider.Listar(query);
            return empresas.Select(EmpresaDTO.Criar);
        }
    }
} 
