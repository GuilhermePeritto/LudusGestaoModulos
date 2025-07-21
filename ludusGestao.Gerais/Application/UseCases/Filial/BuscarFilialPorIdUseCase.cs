using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Filial;
using ludusGestao.Gerais.Domain.Repositories;
using ludusGestao.Gerais.Application.Mappers;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Filial
{
    public class BuscarFilialPorIdUseCase
    {
        private readonly IFilialRepository _repository;
        private readonly FilialMapper _mapper;
        public BuscarFilialPorIdUseCase(IFilialRepository repository)
        {
            _repository = repository;
            _mapper = new FilialMapper();
        }
        public async Task<FilialDTO> Executar(Guid id)
        {
            var entidade = await _repository.BuscarPorId(id);
            if (entidade == null)
                throw new ValidationException("Filial não encontrada.");
            return _mapper.Mapear(entidade);
        }
    }
} 