using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    // DTO usado para atualizar campos específicos no painel
    public class AtualizarCampoDto
    {
        [Required]
        public string Campo { get; set; } = string.Empty;

        [Required]
        public string Valor { get; set; } = string.Empty;
    }

    // DTO usado para criar acessos de Admin e Atendente
    public class CriarAcessoDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Senha { get; set; } = string.Empty;
    }
}
