using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("ONG")]
    // Representa a ONG responsável pelo sistema
    public class Ong
    {
        [Key]
        public int IdOng { get; set; }

        [Required(ErrorMessage = "O nome da ONG e obrigatorio.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "O nome da ONG deve ter entre 3 e 150 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CNPJ e obrigatorio.")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "O CNPJ deve ter 14 digitos.")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "O CNPJ deve conter apenas numeros.")]
        public string Cnpj { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Informe um telefone valido.")]
        [StringLength(11, MinimumLength = 10, ErrorMessage = "O telefone deve ter 10 ou 11 digitos.")]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "O telefone deve conter apenas numeros.")]
        public string? Telefone { get; set; }

        [EmailAddress(ErrorMessage = "Informe um email valido.")]
        [StringLength(150, ErrorMessage = "O email deve ter no maximo 150 caracteres.")]
        public string? Email { get; set; }

        [StringLength(200, ErrorMessage = "O endereco deve ter no maximo 200 caracteres.")]
        public string? Endereco { get; set; }

        // Relaciona administradores vinculados à ONG
        public virtual ICollection<Admin> Administradores { get; set; } = new List<Admin>();

        // Relaciona atendentes vinculados à ONG
        public virtual ICollection<Atendente> Atendentes { get; set; } = new List<Atendente>();
    }
}
