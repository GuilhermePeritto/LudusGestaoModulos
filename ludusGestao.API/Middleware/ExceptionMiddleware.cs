using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using LudusGestao.Shared.Application.Responses;
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

                // Após o next, verificar se há ErroEvento no contexto
                if (context.Items.TryGetValue("ErroEvento", out var erroObj) && erroObj is LudusGestao.Shared.Application.Events.ErroEvento erroEvento)
                {
                    context.Response.StatusCode = erroEvento.StatusCode > 0 ? erroEvento.StatusCode : (int)HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";
                    var resposta = new RespostaBase<object>(null)
                    {
                        Sucesso = false,
                        Mensagem = erroEvento.Mensagem,
                        Erros = erroEvento.Erros ?? new List<string>()
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(resposta));
                }
                // Padronizar resposta para 401/403 se não houver resposta já escrita
                else if ((context.Response.StatusCode == (int)HttpStatusCode.Unauthorized || context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                    && !context.Response.HasStarted)
                {
                    context.Response.ContentType = "application/json";
                    var mensagem = context.Response.StatusCode == (int)HttpStatusCode.Unauthorized ? "Acesso não autorizado." : "Acesso proibido.";
                    var resposta = new RespostaBase<object>(null)
                    {
                        Sucesso = false,
                        Mensagem = mensagem,
                        Erros = new List<string> { mensagem }
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(resposta));
                }
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                var erros = ex.Errors?.Select(e => e.ErrorMessage).ToList() ?? new List<string> { ex.Message };
                var resposta = new RespostaBase<object>(null)
                {
                    Sucesso = false,
                    Mensagem = "Foram encontrados erros de validação.",
                    Erros = erros
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(resposta));
            }
            catch (UnauthorizedAccessException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";
                var resposta = new RespostaBase<object>(null)
                {
                    Sucesso = false,
                    Mensagem = ex.Message ?? "Acesso não autorizado.",
                    Erros = new List<string> { ex.Message }
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(resposta));
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var resposta = new RespostaBase<object>(null)
                {
                    Sucesso = false,
                    Mensagem = "Ocorreu um erro inesperado. Tente novamente mais tarde.",
                    Erros = new List<string> { ex.Message }
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(resposta));
            }
        }
    }
} 