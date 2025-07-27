using BCrypt.Net;

namespace LudusGestao.Shared.Security
{
    public class BCryptPasswordHelper : IPasswordHelper
    {
        public string CriptografarSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
                throw new ArgumentException("Senha não pode ser vazia.", nameof(senha));

            return BCrypt.Net.BCrypt.HashPassword(senha, BCrypt.Net.BCrypt.GenerateSalt(12));
        }

        public bool VerificarSenha(string senha, string senhaCriptografada)
        {
            if (string.IsNullOrWhiteSpace(senha) || string.IsNullOrWhiteSpace(senhaCriptografada))
                return false;

            return BCrypt.Net.BCrypt.Verify(senha, senhaCriptografada);
        }
    }
} 