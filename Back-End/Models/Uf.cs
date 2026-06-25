using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("UF")]
    // Representa a unidade federativa do usuário
    public class Uf
    {
        [Key]
        public int IdUf { get; set; }

        [Required(ErrorMessage = "A sigla da UF e obrigatoria.")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "A sigla da UF deve ter 2 caracteres.")]
        [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "A sigla da UF deve conter duas letras maiusculas.")]
        public string Sigla { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome da UF e obrigatorio.")]
        [StringLength(50, ErrorMessage = "O nome da UF deve ter no maximo 50 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        // Relaciona usuários cadastrados nesta UF
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
