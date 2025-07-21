using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Filial;
using ludusGestao.Gerais.Domain.Entities;
using ludusGestao.Gerais.Domain.Repositories;
using ludusGestao.Gerais.Application.Mappers;
using ludusGestao.Gerais.Application.Validations.Filial;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Filial
{
    public class CriarFilialUseCase
    {
        private readonly IFilialRepository _repository;
        private readonly CriarFilialValidation _validation;
        private readonly FilialMapper _mapper;

        public CriarFilialUseCase(IFilialRepository repository)
        {
            _repository = repository;
            _validation = new CriarFilialValidation();
            _mapper = new FilialMapper();
        }

        public async Task<FilialDTO> Executar(CriarFilialDTO dto)
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