using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Empresa;
using ludusGestao.Gerais.Domain.Entities;
using ludusGestao.Gerais.Domain.Repositories;
using ludusGestao.Gerais.Application.Mappers;
using ludusGestao.Gerais.Application.Validations.Empresa;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Empresa
{
    public class CriarEmpresaUseCase
    {
        private readonly IEmpresaRepository _repository;
        private readonly CriarEmpresaValidation _validation;
        private readonly EmpresaMapper _mapper;

        public CriarEmpresaUseCase(IEmpresaRepository repository)
        {
            _repository = repository;
            _validation = new CriarEmpresaValidation();
            _mapper = new EmpresaMapper();
        }

        public async Task<EmpresaDTO> Executar(CriarEmpresaDTO dto)
        {
            var resultado = _validation.Validate(dto);
            if (!resultado.IsValid)
                throw new ValidationException(resultado.Errors);

            var entidade = _mapper.Mapear(dto);
            await _repository.Adicionar(entidade);
            return _mapper.Mapear(entidade);
        }
    }
} 