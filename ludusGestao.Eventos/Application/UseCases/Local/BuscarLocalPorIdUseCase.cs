using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.DTOs.Local;
using ludusGestao.Eventos.Application.Mappers.local;
using ludusGestao.Eventos.Domain.Providers;
using FluentValidation;
using LudusGestao.Shared.Application.Events;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ludusGestao.Eventos.Application.UseCases.Local
{
    public class BuscarLocalPorIdUseCase
    {
        private readonly ILocalReadProvider _readProvider;
        private readonly LocalMapeador _mapeador;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BuscarLocalPorIdUseCase(ILocalReadProvider readProvider, IHttpContextAccessor httpContextAccessor)
        {
            _readProvider = readProvider;
            _mapeador = new LocalMapeador();
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LocalDTO> Executar(string id)
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

            return _mapeador.Mapear(entidade);
        }
    }
} 