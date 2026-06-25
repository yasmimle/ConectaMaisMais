namespace WebApplication1.Models
{
    // Centraliza os status permitidos para interesses
    public static class StatusInteresseOpcoes
    {
        public const string Pendente = "Pendente";
        public const string Concluido = "Concluído";
        public const string Cancelado = "Cancelado";

        public static readonly string[] Permitidos =
        {
            Pendente,
            Concluido,
            Cancelado
        };

        // Normaliza e valida status recebido do sistema
        public static bool TryNormalizar(string? status, out string statusNormalizado)
        {
            string valor = (status ?? string.Empty).Trim();

            statusNormalizado = Permitidos
                .FirstOrDefault(s => string.Equals(s, valor, StringComparison.OrdinalIgnoreCase))
                ?? string.Empty;

            return !string.IsNullOrWhiteSpace(statusNormalizado);
        }
    }
}
