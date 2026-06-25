using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("STATUS")]
    // Representa o status permitido para uma solicitação
    public class StatusInteresse : IValidatableObject
    {
        [Key]
        public int IdStatus { get; set; }

        [Required(ErrorMessage = "O status e obrigatorio.")]
        [StringLength(50, ErrorMessage = "O status deve ter no maximo 50 caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        // Relaciona interesses que usam este status
        public virtual ICollection<Interesse> Interesses { get; set; } = new List<Interesse>();

        // Valida se o status está entre as opções permitidas
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!StatusInteresseOpcoes.TryNormalizar(Descricao, out _))
            {
                yield return new ValidationResult(
                    "O status deve ser Pendente, Concluído ou Cancelado.",
                    new[] { nameof(Descricao) });
            }
        }
    }
}
