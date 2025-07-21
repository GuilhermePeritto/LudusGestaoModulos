using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Empresa;
using ludusGestao.Gerais.Domain.Repositories;
using ludusGestao.Gerais.Application.Mappers;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Empresa
{
    public class BuscarEmpresaPorIdUseCase
    {
        private readonly IEmpresaRepository _repository;
        private readonly EmpresaMapper _mapper;
        public BuscarEmpresaPorIdUseCase(IEmpresaRepository repository)
        {
            _repository = repository;
            _mapper = new EmpresaMapper();
        }
        public async Task<EmpresaDTO> Executar(Guid id)
        {
            var entidade = await _repository.BuscarPorId(id);
            if (entidade == null)
                throw new ValidationException("Empresa não encontrada.");
            return _mapper.Mapear(entidade);
        }
    }
} 