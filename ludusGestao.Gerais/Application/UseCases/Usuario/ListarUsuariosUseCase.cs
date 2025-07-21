using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Usuario;
using ludusGestao.Gerais.Domain.Repositories;
using ludusGestao.Gerais.Application.Mappers;
using LudusGestao.Shared.Domain.Common;
using System.Linq;

namespace ludusGestao.Gerais.Application.UseCases.Usuario
{
    public class ListarUsuariosUseCase
    {
        private readonly IUsuarioRepository _repository;
        private readonly UsuarioMapper _mapper;
        public ListarUsuariosUseCase(IUsuarioRepository repository)
        {
            _repository = repository;
            _mapper = new UsuarioMapper();
        }
        public async Task<IEnumerable<UsuarioDTO>> Executar()
        {
            var entidades = await _repository.ListarTodos();
            var lista = new List<UsuarioDTO>();
            foreach (var entidade in entidades)
                lista.Add(_mapper.Mapear(entidade));
            return lista;
        }
        public async Task<(IEnumerable<UsuarioDTO> Itens, int Total)> ExecutarPaginado(QueryParamsBase query)
        {
            var (entidades, total) = await _repository.ListarPaginado(query);
            var dtos = entidades.Select(e => _mapper.Mapear(e));
            return (dtos, total);
        }
    }
} 