using System.Collections.Generic;

namespace LudusGestao.Shared.Application.Responses
{
    public class RespostaListaBase<T>
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public IEnumerable<T> Itens { get; set; }
        public int TotalItens { get; set; }
        public int PaginaAtual { get; set; }
        public int TamanhoPagina { get; set; }
        public int TotalPaginas { get; set; }

        public RespostaListaBase(IEnumerable<T> itens, int paginaAtual, int tamanhoPagina, int totalItens, string mensagem = null)
        {
            Sucesso = true;
            Mensagem = mensagem;
            Itens = itens;
            PaginaAtual = paginaAtual;
            TamanhoPagina = tamanhoPagina;
            TotalItens = totalItens;
            TotalPaginas = (int)System.Math.Ceiling(totalItens / (double)tamanhoPagina);
        }
    }
} 