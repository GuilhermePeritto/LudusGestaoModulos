using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using LudusGestao.Shared.Application.Responses;
using FluentValidation;
using System.Text.Json;

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
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                var resposta = new RespostaBase<object>(null)
                {
                    Sucesso = false,
                    Mensagem = string.Join("; ", ex.Errors != null ? ex.Errors : new[] { ex.Message })
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
                    Mensagem = ex.Message ?? "Acesso não autorizado."
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
                    Mensagem = "Ocorreu um erro inesperado. Tente novamente mais tarde."
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(resposta));
            }
        }
    }
} 