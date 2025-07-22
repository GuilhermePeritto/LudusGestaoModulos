using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.DTOs.Local;
using ludusGestao.Eventos.Application.Mappers.local;
using ludusGestao.Eventos.Domain.Providers;
using FluentValidation;
using ludusGestao.Eventos.Application.Validations.Local;
using System;
using LudusGestao.Shared.Application.Events;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace ludusGestao.Eventos.Application.UseCases.Local
{
    public class AtualizarLocalUseCase
    {
        private readonly ILocalWriteProvider _writeProvider;
        private readonly ILocalReadProvider _readProvider;
        private readonly LocalMapeador _mapeador;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AtualizarLocalUseCase(ILocalWriteProvider writeProvider, ILocalReadProvider readProvider, IHttpContextAccessor httpContextAccessor)
        {
            _writeProvider = writeProvider;
            _readProvider = readProvider;
            _mapeador = new LocalMapeador();
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LocalDTO> Executar(string id, AtualizarLocalDTO dto)
        {
            var entidade = await _readProvider.BuscarPorId(Guid.Parse(id));
            if (entidade == null)
            {
                await _httpContextAccessor.PublicarErro(new ErroEvento {
                    Codigo = "NAO_ENCONTRADO",
                    Mensagem = "Local não encontrado.",
                    Erros = new List<string> { "Local não encontrado." },
                    StatusCode = 404
                });
                return null;
            }

            var validation = new AtualizarLocalValidation(_readProvider, id);
            var resultado = await validation.ValidateAsync(dto);
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

            _mapeador.Mapear(dto, entidade);
            await _writeProvider.Atualizar(entidade);
            await _writeProvider.SalvarAlteracoes();
            return _mapeador.Mapear(entidade);
        }
    }
} 