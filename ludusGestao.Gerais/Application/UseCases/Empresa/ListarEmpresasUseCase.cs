using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Empresa;
using ludusGestao.Gerais.Domain.Repositories;
using ludusGestao.Gerais.Application.Mappers;

namespace ludusGestao.Gerais.Application.UseCases.Empresa
{
    public class ListarEmpresasUseCase
    {
        private readonly IEmpresaRepository _repository;
        private readonly EmpresaMapper _mapper;
        public ListarEmpresasUseCase(IEmpresaRepository repository)
        {
            _repository = repository;
            _mapper = new EmpresaMapper();
        }
        public async Task<IEnumerable<EmpresaDTO>> Executar()
        {
            var entidades = await _repository.ListarTodos();
            var lista = new List<EmpresaDTO>();
            foreach (var entidade in entidades)
                lista.Add(_mapper.Mapear(entidade));
            return lista;
        }
    }
} 