using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Empresa;
using ludusGestao.Gerais.Domain.Providers;
using ludusGestao.Gerais.Application.Mappers;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Empresa
{
    public class BuscarEmpresaPorIdUseCase
    {
        private readonly IEmpresaReadProvider _readProvider;
        private readonly EmpresaMapper _mapper;
        public BuscarEmpresaPorIdUseCase(IEmpresaReadProvider readProvider)
        {
            _readProvider = readProvider;
            _mapper = new EmpresaMapper();
        }
        public async Task<EmpresaDTO> Executar(Guid id)
        {
            var entidade = await _readProvider.BuscarPorId(id);
            if (entidade == null)
                throw new FluentValidation.ValidationException("Empresa não encontrada.");
            return _mapper.Mapear(entidade);
        }
    }
} 