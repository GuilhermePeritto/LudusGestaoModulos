using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Empresa;
using ludusGestao.Gerais.Domain.Entities;
using ludusGestao.Gerais.Domain.Providers;
using ludusGestao.Gerais.Application.Mappers;
using ludusGestao.Gerais.Application.Validations.Empresa;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Empresa
{
    public class AtualizarEmpresaUseCase
    {
        private readonly IEmpresaReadProvider _readProvider;
        private readonly IEmpresaWriteProvider _writeProvider;
        private readonly EmpresaMapper _mapper;

        public AtualizarEmpresaUseCase(IEmpresaReadProvider readProvider, IEmpresaWriteProvider writeProvider)
        {
            _readProvider = readProvider;
            _writeProvider = writeProvider;
            _mapper = new EmpresaMapper();
        }

        public async Task<EmpresaDTO> Executar(Guid id, AtualizarEmpresaDTO dto)
        {
            var entidade = await _readProvider.BuscarPorId(id);
            if (entidade == null)
                throw new FluentValidation.ValidationException("Empresa não encontrada.");

            var validation = new AtualizarEmpresaValidation();
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