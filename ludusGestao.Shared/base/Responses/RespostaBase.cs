using System;
using System.Collections;
using System.Collections.Generic;

namespace LudusGestao.Shared.Application.Responses
{
    public class RespostaBase
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public object Conteudo { get; set; }
        public List<string> Erros { get; set; }

        // Campos de paginação (usados apenas se for lista)
        public IEnumerable Itens { get; set; }
        public int? TotalItens { get; set; }
        public int? PaginaAtual { get; set; }
        public int? TamanhoPagina { get; set; }
        public int? TotalPaginas { get; set; }

        public RespostaBase(object conteudo, string mensagem = null, List<string> erros = null, int? totalItens = null, int? paginaAtual = null, int? tamanhoPagina = null)
        {
            Sucesso = erros == null || erros.Count == 0;
            Mensagem = mensagem;
            Erros = erros ?? new List<string>();

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
                Itens = null;
                TotalItens = null;
                PaginaAtual = null;
                TamanhoPagina = null;
                TotalPaginas = null;
            }
        }
    }
} 