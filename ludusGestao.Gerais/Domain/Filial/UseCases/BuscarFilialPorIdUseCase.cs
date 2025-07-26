using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Gerais.Domain.Filial;
using ludusGestao.Gerais.Domain.Filial.Interfaces;

namespace ludusGestao.Gerais.Domain.Filial.UseCases
{
    public class BuscarFilialPorIdUseCase : BaseUseCase, IBuscarFilialPorIdUseCase
    {
        private readonly IFilialReadProvider _provider;

        public BuscarFilialPorIdUseCase(IFilialReadProvider provider, INotificador notificador)
            : base(notificador)
        {
            _provider = provider;
        }

        public async Task<Filial> Executar(Guid id)
        {
            var query = new QueryParamsBase 
            { 
                FilterObjects = new List<FilterObject> 
                { 
                    new FilterObject { Property = "Id", Operator = "eq", Value = id } 
                } 
            };
            
            var filial = await _provider.Buscar(query);
            
            if (filial == null)
            {
                Notificar("Filial não encontrada.");
                return null;
            }

            return filial;
        }
    }
} 