using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Usuario;
using ludusGestao.Gerais.Domain.Entities;
using ludusGestao.Gerais.Domain.Providers;
using ludusGestao.Gerais.Application.Mappers;
using ludusGestao.Gerais.Application.Validations.Usuario;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Usuario
{
    public class CriarUsuarioUseCase
    {
        private readonly IUsuarioReadProvider _readProvider;
        private readonly IUsuarioWriteProvider _writeProvider;
        private readonly CriarUsuarioValidation _validation;
        private readonly UsuarioMapper _mapper;

        public CriarUsuarioUseCase(IUsuarioReadProvider readProvider, IUsuarioWriteProvider writeProvider)
        {
            _readProvider = readProvider;
            _writeProvider = writeProvider;
            _validation = new CriarUsuarioValidation();
            _mapper = new UsuarioMapper();
        }

        public async Task<UsuarioDTO> Executar(CriarUsuarioDTO dto)
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