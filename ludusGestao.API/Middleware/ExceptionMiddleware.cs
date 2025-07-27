using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Responses;
using FluentValidation;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

namespace ludusGestao.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                // Padronizar resposta para 401/403 se não houver resposta já escrita
                if ((context.Response.StatusCode == (int)HttpStatusCode.Unauthorized || context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                    && !context.Response.HasStarted)
                {
                    context.Response.ContentType = "application/json";
                    var mensagem = context.Response.StatusCode == (int)HttpStatusCode.Unauthorized ? "Acesso não autorizado." : "Acesso proibido.";
                    var resposta = new RespostaBase(null, mensagem, new List<string> { mensagem });
                    await context.Response.WriteAsync(JsonSerializer.Serialize(resposta));
                }
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                var erros = ex.Errors?.Select(e => e.ErrorMessage).ToList() ?? new List<string> { ex.Message };
                var resposta = new RespostaBase(null, "Foram encontrados erros de validação.", erros);
                await context.Response.WriteAsync(JsonSerializer.Serialize(resposta));
            }
            catch (UnauthorizedAccessException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";
                var resposta = new RespostaBase(null, ex.Message ?? "Acesso não autorizado.", new List<string> { ex.Message });
                await context.Response.WriteAsync(JsonSerializer.Serialize(resposta));
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var resposta = new RespostaBase(null, "Ocorreu um erro inesperado. Tente novamente mais tarde.", new List<string> { ex.Message });
                await context.Response.WriteAsync(JsonSerializer.Serialize(resposta));
            }
        }
    }
} 