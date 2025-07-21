using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Usuario;
using ludusGestao.Gerais.Domain.Repositories;
using ludusGestao.Gerais.Application.Mappers;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Usuario
{
    public class BuscarUsuarioPorIdUseCase
    {
        private readonly IUsuarioRepository _repository;
        private readonly UsuarioMapper _mapper;
        public BuscarUsuarioPorIdUseCase(IUsuarioRepository repository)
        {
            _repository = repository;
            _mapper = new UsuarioMapper();
        }
        public async Task<UsuarioDTO> Executar(Guid id)
        {
            var entidade = await _repository.BuscarPorId(id);
            if (entidade == null)
                throw new ValidationException("Usuário não encontrado.");
            return _mapper.Mapear(entidade);
        }
    }
} 