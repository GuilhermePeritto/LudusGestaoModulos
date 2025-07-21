using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Filial;
using ludusGestao.Gerais.Domain.Repositories;
using ludusGestao.Gerais.Application.Mappers;
using LudusGestao.Shared.Domain.Common;
using System.Linq;

namespace ludusGestao.Gerais.Application.UseCases.Filial
{
    public class ListarFiliaisUseCase
    {
        private readonly IFilialRepository _repository;
        private readonly FilialMapper _mapper;
        public ListarFiliaisUseCase(IFilialRepository repository)
        {
            _repository = repository;
            _mapper = new FilialMapper();
        }
        public async Task<IEnumerable<FilialDTO>> Executar()
        {
            var entidades = await _repository.ListarTodos();
            var lista = new List<FilialDTO>();
            foreach (var entidade in entidades)
                lista.Add(_mapper.Mapear(entidade));
            return lista;
        }
        public async Task<(IEnumerable<FilialDTO> Itens, int Total)> ExecutarPaginado(QueryParamsBase query)
        {
            var (entidades, total) = await _repository.ListarPaginado(query);
            var dtos = entidades.Select(e => _mapper.Mapear(e));
            return (dtos, total);
        }
    }
} 