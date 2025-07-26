using System;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Empresa;
using ludusGestao.Gerais.Domain.Empresa.Interfaces;
using System.Collections.Generic;

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
            var query = new QueryParamsBase 
            { 
                FilterObjects = new List<FilterObject> 
                { 
                    new FilterObject { Property = "Id", Operator = "eq", Value = id } 
                } 
            };
            
            var empresa = await _provider.Buscar(query);
            
            if (empresa == null)
            {
                Notificar("Empresa não encontrada.");
                return null;
            }

            return empresa;
        }
    }
} 