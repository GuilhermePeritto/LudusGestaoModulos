using System;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.DTOs.Usuario;
using ludusGestao.Gerais.Domain.Entities;
using ludusGestao.Gerais.Domain.Providers;
using ludusGestao.Gerais.Application.Mappers;
using ludusGestao.Gerais.Application.Validations.Usuario;
using FluentValidation;

namespace ludusGestao.Gerais.Application.UseCases.Usuario
{
    public class AtualizarUsuarioUseCase
    {
        private readonly IUsuarioReadProvider _readProvider;
        private readonly IUsuarioWriteProvider _writeProvider;
        private readonly UsuarioMapper _mapper;

        public AtualizarUsuarioUseCase(IUsuarioReadProvider readProvider, IUsuarioWriteProvider writeProvider)
        {
            _readProvider = readProvider;
            _writeProvider = writeProvider;
            _mapper = new UsuarioMapper();
        }

        public async Task<UsuarioDTO> Executar(Guid id, AtualizarUsuarioDTO dto)
        {
            var entidade = await _readProvider.BuscarPorId(id);
            if (entidade == null)
                throw new FluentValidation.ValidationException("Usuário não encontrado.");

            var validation = new AtualizarUsuarioValidation();
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