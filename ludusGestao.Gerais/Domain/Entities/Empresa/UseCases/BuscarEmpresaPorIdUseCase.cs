using System;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.QueryParams.Helpers;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Gerais.Domain.Empresa;
using ludusGestao.Gerais.Domain.Empresa.Interfaces;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Gerais.Domain.Empresa.UseCases
{
    public class BuscarEmpresaPorIdUseCase : BaseUseCase, IBuscarEmpresaPorIdUseCase
    {
        private readonly IEmpresaReadProvider _provider;

        public BuscarEmpresaPorIdUseCase(IEmpresaReadProvider provider, INotificador notificador)
            : base(notificador)
        {
            _provider = provider;
        }

        public async Task<Empresa> Executar(Guid id)
        {
            var empresa = await _provider.Buscar(QueryParamsHelper.FiltrarPorId(id));
            
            if (empresa == null)
            {
                Notificar("Empresa não encontrada.");
                return null;
            }

            return empresa;
        }
    }
} 