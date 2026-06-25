using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace WebApplication1.Models
{
    [NotMapped]
    // Classe base para usuários que acessam o sistema com senha
    public abstract class UsuarioSistema : Pessoa
    {
        [Required]
        [StringLength(256)]
        public string SenhaHash { get; private set; } = string.Empty;

        [Required]
        [StringLength(128)]
        public string SenhaSalt { get; private set; } = string.Empty;

        [NotMapped]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        public string? Senha { get; set; }

        // Define senha criptografada para login
        public void DefinirSenha(string senha)
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

            SenhaSalt = Convert.ToBase64String(salt);
            SenhaHash = Convert.ToBase64String(hash);
        }

        // Verifica senha informada contra o hash salvo
        public bool VerificarSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha) ||
                string.IsNullOrWhiteSpace(SenhaHash) ||
                string.IsNullOrWhiteSpace(SenhaSalt))
            {
                return false;
            }

            byte[] salt = Convert.FromBase64String(SenhaSalt);
            byte[] hashInformado = Rfc2898DeriveBytes.Pbkdf2(
                senha,
                salt,
                100000,
                HashAlgorithmName.SHA256,
                32);

            byte[] hashSalvo = Convert.FromBase64String(SenhaHash);
            return CryptographicOperations.FixedTimeEquals(hashInformado, hashSalvo);
        }
    }
}
