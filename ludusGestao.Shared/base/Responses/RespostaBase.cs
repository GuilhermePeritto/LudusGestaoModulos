using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LudusGestao.Shared.Domain.Responses
{
    public class RespostaBase
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object Conteudo { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> Erros { get; set; }

        // Campos de paginação (usados apenas se for lista)
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable Itens { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TotalItens { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? PaginaAtual { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TamanhoPagina { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TotalPaginas { get; set; }

        public RespostaBase(object conteudo, string mensagem = null, List<string> erros = null, int? totalItens = null, int? paginaAtual = null, int? tamanhoPagina = null)
        {
            Sucesso = erros == null || erros.Count == 0;
            Mensagem = mensagem;
            Erros = erros ?? new List<string>();

            // Se houver erros, não definir os outros campos
            if (!Sucesso)
            {
                return;
            }

            if (conteudo is IEnumerable enumerable && !(conteudo is string))
            {
                var itensList = new List<object>();
                foreach (var item in enumerable)
                    itensList.Add(item);
                Itens = itensList;
                TotalItens = totalItens ?? itensList.Count;
                PaginaAtual = paginaAtual ?? 1;
                TamanhoPagina = tamanhoPagina ?? itensList.Count;
                TotalPaginas = TamanhoPagina > 0 ? (int)Math.Ceiling((TotalItens ?? 0) / (double)TamanhoPagina.Value) : 1;
                Conteudo = null;
            }
            else
            {
                Conteudo = conteudo;
                // Não definir os campos de paginação quando não for lista
            }
        }
    }
} 