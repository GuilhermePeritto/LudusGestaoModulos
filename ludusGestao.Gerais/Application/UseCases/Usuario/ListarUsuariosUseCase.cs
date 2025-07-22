using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Usuario;
using ludusGestao.Gerais.Domain.Providers;
using ludusGestao.Gerais.Application.Mappers;
using LudusGestao.Shared.Domain.Common;
using System.Linq;

namespace ludusGestao.Gerais.Application.UseCases.Usuario
{
    public class ListarUsuariosUseCase
    {
        private readonly IUsuarioReadProvider _readProvider;
        private readonly UsuarioMapper _mapper;
        public ListarUsuariosUseCase(IUsuarioReadProvider readProvider)
        {
            _readProvider = readProvider;
            _mapper = new UsuarioMapper();
        }
        public async Task<IEnumerable<UsuarioDTO>> Executar()
        {
            var entidades = await _readProvider.ListarTodos();
            var lista = new List<UsuarioDTO>();
            foreach (var entidade in entidades)
                lista.Add(_mapper.Mapear(entidade));
            return lista;
        }
        public async Task<(IEnumerable<UsuarioDTO> Itens, int Total)> ExecutarPaginado(QueryParamsBase query)
        {
            var resultado = await _readProvider.ListarPaginado(query);
            var dtos = resultado.Itens.Select(e => _mapper.Mapear(e));
            return (dtos, resultado.Total);
        }
    }
} 