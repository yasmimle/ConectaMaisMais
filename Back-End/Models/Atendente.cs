using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("ATENDENTE")]
    // Representa atendente com permissão de acesso ao painel Atendente
    public class Atendente : UsuarioSistema
    {
        [Key]
        public int IdAtendente { get; set; }

        [Required(ErrorMessage = "A ONG e obrigatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Informe uma ONG valida.")]
        public int IdOng { get; set; }

        [ForeignKey(nameof(IdOng))]
        public virtual Ong? Ong { get; set; }

        [NotMapped]
        // Especializa o tipo da classe base UsuarioSistema
        public override string TipoPessoa => "Atendente";

        // Exibe dados principais do atendente
        public override string ExibirDados()
        {
            return $"Atendente: {Nome} | Email: {Email}";
        }
    }
}
