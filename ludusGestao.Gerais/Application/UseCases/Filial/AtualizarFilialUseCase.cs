using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Filial;
using ludusGestao.Gerais.Domain.Entities;
using ludusGestao.Gerais.Domain.Providers;
using ludusGestao.Gerais.Application.Mappers;
using ludusGestao.Gerais.Application.Validations.Filial;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Filial
{
    public class AtualizarFilialUseCase
    {
        private readonly IFilialReadProvider _readProvider;
        private readonly IFilialWriteProvider _writeProvider;
        private readonly FilialMapper _mapper;

        public AtualizarFilialUseCase(IFilialReadProvider readProvider, IFilialWriteProvider writeProvider)
        {
            _readProvider = readProvider;
            _writeProvider = writeProvider;
            _mapper = new FilialMapper();
        }

        public async Task<FilialDTO> Executar(Guid id, AtualizarFilialDTO dto)
        {
            var entidade = await _readProvider.BuscarPorId(id);
            if (entidade == null)
                throw new FluentValidation.ValidationException("Filial não encontrada.");

            var validation = new AtualizarFilialValidation();
            var resultado = validation.Validate(dto);
            if (!resultado.IsValid)
                throw new FluentValidation.ValidationException(resultado.Errors);

            _mapper.Mapear(dto, entidade);
            await _writeProvider.Atualizar(entidade);
            await _writeProvider.SalvarAlteracoes();
            return _mapper.Mapear(entidade);
        }
    }
} 