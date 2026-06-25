using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("USUARIO")]
    // Representa o usuário público cadastrado no sistema
    public class Usuario : Pessoa, IValidatableObject
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "O CPF e obrigatorio.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 digitos.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter apenas numeros.")]
        public string Cpf { get; set; } = string.Empty;

        [Required(ErrorMessage = "O endereco e obrigatorio.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "O endereco deve ter entre 5 e 200 caracteres.")]
        public string Endereco { get; set; } = string.Empty;

        [Required(ErrorMessage = "A cidade e obrigatoria.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "A cidade deve ter entre 2 e 100 caracteres.")]
        [RegularExpression(@"^[A-Za-zÀ-ÿ\s]+$", ErrorMessage = "A cidade deve conter apenas letras e espacos.")]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data de nascimento e obrigatoria.")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "O tipo de participacao e obrigatorio.")]
        [StringLength(20, ErrorMessage = "O tipo de participacao deve ter no maximo 20 caracteres.")]
        public string TipoParticipacao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A UF e obrigatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Informe uma UF valida.")]
        public int IdUf { get; set; }

        [ForeignKey(nameof(IdUf))]
        public virtual Uf? Uf { get; set; }

        public virtual ICollection<Interesse> Interesses { get; set; } = new List<Interesse>();

        [NotMapped]
        // Especializa o tipo da classe base Pessoa
        public override string TipoPessoa => "Usuario";

        // Exibe dados principais do usuário
        public override string ExibirDados()
        {
            return $"Usuario: {Nome} | CPF: {Cpf} | Email: {Email}";
        }

        // Valida regras de negócio do usuário
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!CpfValido(Cpf))
            {
                yield return new ValidationResult("CPF invalido.", new[] { nameof(Cpf) });
            }

            if (DataNascimento.Date >= DateTime.Today)
            {
                yield return new ValidationResult("A data de nascimento deve ser anterior a data atual.", new[] { nameof(DataNascimento) });
            }

            var tiposValidos = new[] { "Doador", "Receptor", "Ambos" };
            if (!tiposValidos.Contains(TipoParticipacao, StringComparer.OrdinalIgnoreCase))
            {
                yield return new ValidationResult("O tipo de participacao deve ser Doador, Receptor ou Ambos.", new[] { nameof(TipoParticipacao) });
            }
        }

        // Valida CPF pelo cálculo dos dígitos verificadores
        private static bool CpfValido(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11 || !cpf.All(char.IsDigit))
            {
                return false;
            }

            if (cpf.All(c => c == cpf[0]))
            {
                return false;
            }

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf[..9];
            int soma = 0;

            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            }

            int resto = soma % 11;
            int digito = resto < 2 ? 0 : 11 - resto;
            tempCpf += digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            }

            resto = soma % 11;
            digito = resto < 2 ? 0 : 11 - resto;

            return cpf.EndsWith(digito.ToString(), StringComparison.Ordinal);
        }
    }
}
