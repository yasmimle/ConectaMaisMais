using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("ADMIN")]
    // Representa administrador com permissão de acesso ao painel Admin
    public class Admin : UsuarioSistema
    {
        [Key]
        public int IdAdmin { get; set; }

        [Required(ErrorMessage = "A ONG e obrigatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Informe uma ONG valida.")]
        public int IdOng { get; set; }

        [ForeignKey(nameof(IdOng))]
        public virtual Ong? Ong { get; set; }

        [NotMapped]
        // Especializa o tipo da classe base UsuarioSistema
        public override string TipoPessoa => "Administrador";

        // Exibe dados principais do administrador
        public override string ExibirDados()
        {
            return $"Administrador: {Nome} | Email: {Email}";
        }
    }
}
