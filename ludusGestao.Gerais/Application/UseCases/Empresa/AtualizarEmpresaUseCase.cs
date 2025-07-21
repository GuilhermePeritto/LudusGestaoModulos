using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Empresa;
using ludusGestao.Gerais.Domain.Entities;
using ludusGestao.Gerais.Domain.Repositories;
using ludusGestao.Gerais.Application.Mappers;
using ludusGestao.Gerais.Application.Validations.Empresa;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Empresa
{
    public class AtualizarEmpresaUseCase
    {
        private readonly IEmpresaRepository _repository;
        private readonly EmpresaMapper _mapper;

        public AtualizarEmpresaUseCase(IEmpresaRepository repository)
        {
            _repository = repository;
            _mapper = new EmpresaMapper();
        }

        public async Task<EmpresaDTO> Executar(Guid id, AtualizarEmpresaDTO dto)
        {
            var entidade = await _repository.BuscarPorId(id);
            if (entidade == null)
                throw new ValidationException("Empresa não encontrada.");

            var validation = new AtualizarEmpresaValidation();
            var resultado = validation.Validate(dto);
            if (!resultado.IsValid)
                throw new ValidationException(resultado.Errors);

            _mapper.Mapear(dto, entidade);
            await _repository.Atualizar(entidade);
            return _mapper.Mapear(entidade);
        }
    }
} 