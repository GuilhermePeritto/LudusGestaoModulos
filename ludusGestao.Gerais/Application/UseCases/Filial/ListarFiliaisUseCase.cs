using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Filial;
using ludusGestao.Gerais.Domain.Providers;
using ludusGestao.Gerais.Application.Mappers;
using LudusGestao.Shared.Domain.Common;
using System.Linq;

namespace ludusGestao.Gerais.Application.UseCases.Filial
{
    public class ListarFiliaisUseCase
    {
        private readonly IFilialReadProvider _readProvider;
        private readonly FilialMapper _mapper;
        public ListarFiliaisUseCase(IFilialReadProvider readProvider)
        {
            _readProvider = readProvider;
            _mapper = new FilialMapper();
        }
        public async Task<IEnumerable<FilialDTO>> Executar()
        {
            var entidades = await _readProvider.ListarTodos();
            var lista = new List<FilialDTO>();
            foreach (var entidade in entidades)
                lista.Add(_mapper.Mapear(entidade));
            return lista;
        }
        public async Task<(IEnumerable<FilialDTO> Itens, int Total)> ExecutarPaginado(QueryParamsBase query)
        {
            var resultado = await _readProvider.ListarPaginado(query);
            var dtos = resultado.Itens.Select(e => _mapper.Mapear(e));
            return (dtos, resultado.Total);
        }
    }
} 