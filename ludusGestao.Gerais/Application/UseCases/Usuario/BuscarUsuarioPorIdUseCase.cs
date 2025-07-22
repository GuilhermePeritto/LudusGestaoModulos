using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Usuario;
using ludusGestao.Gerais.Domain.Providers;
using ludusGestao.Gerais.Application.Mappers;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Usuario
{
    public class BuscarUsuarioPorIdUseCase
    {
        private readonly IUsuarioReadProvider _readProvider;
        private readonly UsuarioMapper _mapper;
        public BuscarUsuarioPorIdUseCase(IUsuarioReadProvider readProvider)
        {
            _readProvider = readProvider;
            _mapper = new UsuarioMapper();
        }
        public async Task<UsuarioDTO> Executar(Guid id)
        {
            var entidade = await _readProvider.BuscarPorId(id);
            if (entidade == null)
                throw new FluentValidation.ValidationException("Usuário não encontrado.");
            return _mapper.Mapear(entidade);
        }
    }
} 