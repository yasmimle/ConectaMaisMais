using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("INTERESSE")]
    // Representa uma solicitação criada por um usuário
    public class Interesse : IValidatableObject
    {
        [Key]
        public int IdInteresse { get; set; }

        [Required(ErrorMessage = "O usuario e obrigatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Informe um usuario valido.")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "O tipo do interesse e obrigatorio.")]
        [StringLength(20, ErrorMessage = "O tipo deve ter no maximo 20 caracteres.")]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descricao e obrigatoria.")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "A descricao deve ter entre 5 e 500 caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O status e obrigatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Informe um status valido.")]
        public int IdStatus { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DataRegistro { get; set; } = DateTime.Now;

        [ForeignKey(nameof(IdUsuario))]
        public virtual Usuario? Usuario { get; set; }

        [ForeignKey(nameof(IdStatus))]
        public virtual StatusInteresse? Status { get; set; }

        // Valida regras de negócio do interesse
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var tiposValidos = new[] { "Doador", "Receptor", "Ambos" };
            if (!tiposValidos.Contains(Tipo, StringComparer.OrdinalIgnoreCase))
            {
                yield return new ValidationResult("O tipo deve ser Doador, Receptor ou Ambos.", new[] { nameof(Tipo) });
            }

            if (Status != null && !StatusInteresseOpcoes.TryNormalizar(Status.Descricao, out _))
            {
                yield return new ValidationResult("O status deve ser Pendente, Concluído ou Cancelado.", new[] { nameof(Status) });
            }
        }
    }
}
