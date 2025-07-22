using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Empresa;
using ludusGestao.Gerais.Domain.Providers;
using ludusGestao.Gerais.Application.Mappers;
using LudusGestao.Shared.Domain.Common;
using System.Linq;

namespace ludusGestao.Gerais.Application.UseCases.Empresa
{
    public class ListarEmpresasUseCase
    {
        private readonly IEmpresaReadProvider _readProvider;
        private readonly EmpresaMapper _mapper;
        public ListarEmpresasUseCase(IEmpresaReadProvider readProvider)
        {
            _readProvider = readProvider;
            _mapper = new EmpresaMapper();
        }
        public async Task<IEnumerable<EmpresaDTO>> Executar()
        {
            var entidades = await _readProvider.ListarTodos();
            var lista = new List<EmpresaDTO>();
            foreach (var entidade in entidades)
                lista.Add(_mapper.Mapear(entidade));
            return lista;
        }
        public async Task<(IEnumerable<EmpresaDTO> Itens, int Total)> ExecutarPaginado(QueryParamsBase query)
        {
            var resultado = await _readProvider.ListarPaginado(query);
            var dtos = resultado.Itens.Select(e => _mapper.Mapear(e));
            return (dtos, resultado.Total);
        }
    }
} 