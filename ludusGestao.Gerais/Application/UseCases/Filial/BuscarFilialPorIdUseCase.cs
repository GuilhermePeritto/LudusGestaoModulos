using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Filial;
using ludusGestao.Gerais.Domain.Providers;
using ludusGestao.Gerais.Application.Mappers;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Filial
{
    public class BuscarFilialPorIdUseCase
    {
        private readonly IFilialReadProvider _readProvider;
        private readonly FilialMapper _mapper;
        public BuscarFilialPorIdUseCase(IFilialReadProvider readProvider)
        {
            _readProvider = readProvider;
            _mapper = new FilialMapper();
        }
        public async Task<FilialDTO> Executar(Guid id)
        {
            var entidade = await _readProvider.BuscarPorId(id);
            if (entidade == null)
                throw new FluentValidation.ValidationException("Filial não encontrada.");
            return _mapper.Mapear(entidade);
        }
    }
} 