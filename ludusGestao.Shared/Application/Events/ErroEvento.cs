using System;
using System.Collections.Generic;

namespace LudusGestao.Shared.Application.Events
{
    public class ErroEvento
    {
        public string Codigo { get; set; }
        public string Mensagem { get; set; }
        public Exception Excecao { get; set; }
        public int StatusCode { get; set; }
        public List<string> Erros { get; set; }
        public object Contexto { get; set; }
    }
} 