using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Filial;
using ludusGestao.Gerais.Domain.Entities;
using ludusGestao.Gerais.Domain.Providers;
using ludusGestao.Gerais.Application.Mappers;
using ludusGestao.Gerais.Application.Validations.Filial;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Filial
{
    public class CriarFilialUseCase
    {
        private readonly IFilialReadProvider _readProvider;
        private readonly IFilialWriteProvider _writeProvider;
        private readonly CriarFilialValidation _validation;
        private readonly FilialMapper _mapper;

        public CriarFilialUseCase(IFilialReadProvider readProvider, IFilialWriteProvider writeProvider)
        {
            _readProvider = readProvider;
            _writeProvider = writeProvider;
            _validation = new CriarFilialValidation();
            _mapper = new FilialMapper();
        }

        public async Task<FilialDTO> Executar(CriarFilialDTO dto)
        {
            var resultado = _validation.Validate(dto);
            if (!resultado.IsValid)
                throw new FluentValidation.ValidationException(resultado.Errors);

            var entidade = _mapper.Mapear(dto);
            await _writeProvider.Adicionar(entidade);
            await _writeProvider.SalvarAlteracoes();
            return _mapper.Mapear(entidade);
        }
    }
} 