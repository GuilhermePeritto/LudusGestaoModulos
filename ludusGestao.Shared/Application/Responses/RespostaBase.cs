using System.Collections.Generic;

namespace LudusGestao.Shared.Application.Responses
{
    public class RespostaBase<T>
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public T Conteudo { get; set; }
        public List<string> Erros { get; set; }

        public RespostaBase(T conteudo, string mensagem = null, List<string> erros = null)
        {
            Sucesso = true;
            Mensagem = mensagem;
            Conteudo = conteudo;
            Erros = erros ?? new List<string>();
        }
    }
} 