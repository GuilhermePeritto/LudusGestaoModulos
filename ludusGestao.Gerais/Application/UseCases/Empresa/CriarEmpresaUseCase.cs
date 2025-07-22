using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Empresa;
using ludusGestao.Gerais.Domain.Entities;
using ludusGestao.Gerais.Domain.Providers;
using ludusGestao.Gerais.Application.Mappers;
using ludusGestao.Gerais.Application.Validations.Empresa;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Empresa
{
    public class CriarEmpresaUseCase
    {
        private readonly IEmpresaReadProvider _readProvider;
        private readonly IEmpresaWriteProvider _writeProvider;
        private readonly CriarEmpresaValidation _validation;
        private readonly EmpresaMapper _mapper;

        public CriarEmpresaUseCase(IEmpresaReadProvider readProvider, IEmpresaWriteProvider writeProvider)
        {
            _readProvider = readProvider;
            _writeProvider = writeProvider;
            _validation = new CriarEmpresaValidation();
            _mapper = new EmpresaMapper();
        }

        public async Task<EmpresaDTO> Executar(CriarEmpresaDTO dto)
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