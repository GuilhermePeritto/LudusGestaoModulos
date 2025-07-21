using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Filial;
using ludusGestao.Gerais.Domain.Entities;
using ludusGestao.Gerais.Domain.Repositories;
using ludusGestao.Gerais.Application.Mappers;
using ludusGestao.Gerais.Application.Validations.Filial;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Filial
{
    public class AtualizarFilialUseCase
    {
        private readonly IFilialRepository _repository;
        private readonly FilialMapper _mapper;

        public AtualizarFilialUseCase(IFilialRepository repository)
        {
            _repository = repository;
            _mapper = new FilialMapper();
        }

        public async Task<FilialDTO> Executar(Guid id, AtualizarFilialDTO dto)
        {
            var entidade = await _repository.BuscarPorId(id);
            if (entidade == null)
                throw new ValidationException("Filial não encontrada.");

            var validation = new AtualizarFilialValidation();
            var resultado = validation.Validate(dto);
            if (!resultado.IsValid)
                throw new ValidationException(resultado.Errors);

            _mapper.Mapear(dto, entidade);
            await _repository.Atualizar(entidade);
            return _mapper.Mapear(entidade);
        }
    }
} 