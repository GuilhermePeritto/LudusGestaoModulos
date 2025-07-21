namespace LudusGestao.Shared.Application.Responses
{
    public class RespostaBase<T>
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public T Conteudo { get; set; }

        public RespostaBase(T conteudo, string mensagem = null)
        {
            Sucesso = true;
            Mensagem = mensagem;
            Conteudo = conteudo;
        }
    }
} 