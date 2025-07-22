using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.Providers;
using FluentValidation;
using ludusGestao.Eventos.Domain.Specifications;
using System;
using LudusGestao.Shared.Application.Events;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ludusGestao.Eventos.Application.UseCases.Local
{
    public class RemoverLocalUseCase
    {
        private readonly ILocalWriteProvider _writeProvider;
        private readonly ILocalReadProvider _readProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RemoverLocalUseCase(ILocalWriteProvider writeProvider, ILocalReadProvider readProvider, IHttpContextAccessor httpContextAccessor)
        {
            _writeProvider = writeProvider;
            _readProvider = readProvider;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Executar(string id)
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
                return false;
            }

            var disponivelSpec = new LocalDisponivelSpecification();
            if (!disponivelSpec.IsSatisfiedBy(entidade))
            {
                await _httpContextAccessor.PublicarErro(new ErroEvento {
                    Codigo = "VALIDACAO",
                    Mensagem = disponivelSpec.ErrorMessage,
                    Erros = new List<string> { disponivelSpec.ErrorMessage },
                    StatusCode = 400
                });
                return false;
            }

            await _writeProvider.Remover(entidade.Id);
            await _writeProvider.SalvarAlteracoes();
            return true;
        }
    }
} 