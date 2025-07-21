using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Usuario;
using ludusGestao.Gerais.Domain.Entities;
using ludusGestao.Gerais.Domain.Repositories;
using ludusGestao.Gerais.Application.Mappers;
using ludusGestao.Gerais.Application.Validations.Usuario;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Usuario
{
    public class AtualizarUsuarioUseCase
    {
        private readonly IUsuarioRepository _repository;
        private readonly UsuarioMapper _mapper;

        public AtualizarUsuarioUseCase(IUsuarioRepository repository)
        {
            _repository = repository;
            _mapper = new UsuarioMapper();
        }

        public async Task<UsuarioDTO> Executar(Guid id, AtualizarUsuarioDTO dto)
        {
            var entidade = await _repository.BuscarPorId(id);
            if (entidade == null)
                throw new ValidationException("Usuário não encontrado.");

            var validation = new AtualizarUsuarioValidation();
            var resultado = validation.Validate(dto);
            if (!resultado.IsValid)
                throw new ValidationException(resultado.Errors);

            _mapper.Mapear(dto, entidade);
            await _repository.Atualizar(entidade);
            return _mapper.Mapear(entidade);
        }
    }
} 