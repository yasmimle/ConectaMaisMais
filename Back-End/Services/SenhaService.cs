using System.Security.Cryptography;

namespace WebApplication1.Services
{
    // Serviço responsável por gerar hash seguro de senhas
    public static class SenhaService
    {
        // Criptografa senha usando salt e PBKDF2
        public static (string Hash, string Salt) Criptografar(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
            {
                throw new ArgumentException("A senha nao pode ficar vazia.", nameof(senha));
            }

            byte[] salt = RandomNumberGenerator.GetBytes(16);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                senha,
                salt,
                100000,
                HashAlgorithmName.SHA256,
                32);

            return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
        }
    }
}
