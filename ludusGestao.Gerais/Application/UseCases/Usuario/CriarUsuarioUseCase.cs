using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Usuario;
using ludusGestao.Gerais.Domain.Entities;
using ludusGestao.Gerais.Domain.Repositories;
using ludusGestao.Gerais.Application.Mappers;
using ludusGestao.Gerais.Application.Validations.Usuario;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Usuario
{
    public class CriarUsuarioUseCase
    {
        private readonly IUsuarioRepository _repository;
        private readonly CriarUsuarioValidation _validation;
        private readonly UsuarioMapper _mapper;

        public CriarUsuarioUseCase(IUsuarioRepository repository)
        {
            _repository = repository;
            _validation = new CriarUsuarioValidation();
            _mapper = new UsuarioMapper();
        }

        public async Task<UsuarioDTO> Executar(CriarUsuarioDTO dto)
        {
            var resultado = _validation.Validate(dto);
            if (!resultado.IsValid)
                throw new ValidationException(resultado.Errors);

            var entidade = _mapper.Mapear(dto);
            await _repository.Adicionar(entidade);
            return _mapper.Mapear(entidade);
        }
    }
} 