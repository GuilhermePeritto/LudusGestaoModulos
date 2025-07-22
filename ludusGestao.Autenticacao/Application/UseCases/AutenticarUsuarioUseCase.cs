namespace ludusGestao.Autenticacao.Application.UseCases
{
    public class AutenticarUsuarioUseCase
    {
        // Exemplo de método de autenticação
        public bool Autenticar(string login, string senha)
        {
            // Lógica de autenticação fictícia
            return login == "admin" && senha == "123";
        }
    }
} 