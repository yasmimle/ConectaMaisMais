using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    // Dados recebidos no cadastro público de usuário
    public class CadastroViewModel
    {
        [Required(ErrorMessage = "O nome e obrigatorio.")]
        [StringLength(100, MinimumLength = 3)]
        public string Nome { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Informe um email valido.")]
        [StringLength(150)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "O telefone e obrigatorio.")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "O telefone deve ter 10 ou 11 digitos.")]
        public string Telefone { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF e obrigatorio.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve ter 11 digitos.")]
        public string Cpf { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Endereco { get; set; }

        [StringLength(100)]
        public string? Cidade { get; set; }

        [StringLength(2)]
        public string? Estado { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DataNascimento { get; set; }

        public string? TipoParticipacao { get; set; }

        [Required(ErrorMessage = "A necessidade e obrigatoria.")]
        [StringLength(500)]
        public string Necessidade { get; set; } = string.Empty;

        public string? Senha { get; set; }
    }
}
