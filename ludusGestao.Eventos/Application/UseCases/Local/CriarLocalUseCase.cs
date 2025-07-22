using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.DTOs.Local;
using ludusGestao.Eventos.Application.Mappers.local;
using ludusGestao.Eventos.Application.Validations.Local;
using ludusGestao.Eventos.Domain.Providers;
using FluentValidation;
using ludusGestao.Eventos.Domain.Specifications;
using LudusGestao.Shared.Application.Events;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace ludusGestao.Eventos.Application.UseCases.Local
{
    public class CriarLocalUseCase
    {
        private readonly ILocalWriteProvider _writeProvider;
        private readonly ILocalReadProvider _readProvider;
        private readonly CriarLocalValidation _validation;
        private readonly LocalMapeador _mapeador;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CriarLocalUseCase(ILocalWriteProvider writeProvider, ILocalReadProvider readProvider, IHttpContextAccessor httpContextAccessor)
        {
            _writeProvider = writeProvider;
            _readProvider = readProvider;
            _validation = new CriarLocalValidation(_readProvider);
            _mapeador = new LocalMapeador();
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LocalDTO> Executar(CriarLocalDTO dto)
        {
            var resultado = _validation.Validate(dto);
            if (!resultado.IsValid)
            {
                await _httpContextAccessor.PublicarErro(new ErroEvento {
                    Codigo = "VALIDACAO",
                    Mensagem = "Foram encontrados erros de validação.",
                    Erros = resultado.Errors.Select(e => e.ErrorMessage).ToList(),
                    StatusCode = 400
                });
                return null;
            }

            var entidade = _mapeador.Mapear(dto);

            // Validação de capacidade suficiente
            var capacidadeSpec = new LocalCapacidadeSuficienteSpecification(dto.Capacidade);
            if (!capacidadeSpec.IsSatisfiedBy(entidade))
            {
                await _httpContextAccessor.PublicarErro(new ErroEvento {
                    Codigo = "VALIDACAO",
                    Mensagem = capacidadeSpec.ErrorMessage,
                    Erros = new List<string> { capacidadeSpec.ErrorMessage },
                    StatusCode = 400
                });
                return null;
            }

            // Validação de disponibilidade
            var disponivelSpec = new LocalDisponivelSpecification();
            if (!disponivelSpec.IsSatisfiedBy(entidade))
            {
                await _httpContextAccessor.PublicarErro(new ErroEvento {
                    Codigo = "VALIDACAO",
                    Mensagem = disponivelSpec.ErrorMessage,
                    Erros = new List<string> { disponivelSpec.ErrorMessage },
                    StatusCode = 400
                });
                return null;
            }

            await _writeProvider.Adicionar(entidade);
            await _writeProvider.SalvarAlteracoes();
            return _mapeador.Mapear(entidade);
        }
    }
} 