using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [NotMapped]
    // Classe base para pessoas do sistema
    public abstract class Pessoa
    {
        [Required(ErrorMessage = "O nome e obrigatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
        [RegularExpression(@"^[A-Za-zÀ-ÿ\s]+$", ErrorMessage = "O nome deve conter apenas letras e espacos.")]
        public string Nome { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Informe um email valido.")]
        [StringLength(150, ErrorMessage = "O email deve ter no maximo 150 caracteres.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Informe um telefone valido.")]
        [StringLength(11, MinimumLength = 10, ErrorMessage = "O telefone deve ter 10 ou 11 digitos.")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "O telefone deve conter apenas numeros.")]
        public string? Telefone { get; set; }

        [NotMapped]
        // Permite identificar o tipo da pessoa por polimorfismo
        public virtual string TipoPessoa => "Pessoa";

        // Exibe dados principais da pessoa
        public virtual string ExibirDados()
        {
            return $"Nome: {Nome} | Email: {Email} | Telefone: {Telefone}";
        }
    }
}
