using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    // Dados necessários para autenticação no sistema
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O email e obrigatorio.")]
        [EmailAddress(ErrorMessage = "Informe um email valido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha e obrigatoria.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = string.Empty;
    }
}
