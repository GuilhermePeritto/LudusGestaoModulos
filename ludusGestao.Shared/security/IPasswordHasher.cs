namespace LudusGestao.Shared.Security
{
    public interface IPasswordHelper
    {
        string CriptografarSenha(string senha);
        bool VerificarSenha(string senha, string senhaCriptografada);
    }
} 