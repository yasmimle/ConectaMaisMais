using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Authorize(Roles = "Atendente")]
    [Route("api/atendente")]
    // API responsável pelas ações do painel Atendente
    public class AtendenteApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AtendenteApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("usuarios")]
        // Lista usuários cadastrados
        public async Task<IActionResult> ListarUsuarios()
        {
            var usuarios = await _context.Usuarios
                .AsNoTracking()
                .OrderBy(u => u.Nome)
                .Select(u => new
                {
                    id = u.IdUsuario,
                    nome = u.Nome,
                    cpf = u.Cpf,
                    email = u.Email,
                    telefone = u.Telefone,
                    endereco = u.Endereco
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        [HttpPut("usuarios/{id:int}")]
        // Edita dados permitidos do usuário
        public async Task<IActionResult> EditarUsuario(int id, AtualizarCampoDto dados)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { mensagem = "Dados invalidos." });
            }

            Usuario? usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound(new { mensagem = "Usuario nao encontrado." });
            }

            string valor = dados.Valor.Trim();

            // Valida o campo solicitado antes de atualizar
            switch (dados.Campo.Trim().ToLowerInvariant())
            {
                case "telefone":
                    string telefone = ApenasNumeros(valor);
                    if (telefone.Length is < 10 or > 11)
                    {
                        return BadRequest(new { mensagem = "Telefone invalido." });
                    }
                    usuario.Telefone = telefone;
                    break;

                case "email":
                    if (!new EmailAddressAttribute().IsValid(valor))
                    {
                        return BadRequest(new { mensagem = "Email invalido." });
                    }
                    usuario.Email = valor;
                    break;

                case "endereco":
                    if (valor.Length < 5)
                    {
                        return BadRequest(new { mensagem = "Endereco invalido." });
                    }
                    usuario.Endereco = valor;
                    break;

                default:
                    return BadRequest(new { mensagem = "Campo nao permitido." });
            }

            await _context.SaveChangesAsync();
            return Ok(new { sucesso = true });
        }

        [HttpGet("interesses")]
        // Lista interesses cadastrados
        public async Task<IActionResult> ListarInteresses()
        {
            var interesses = await _context.Interesses
                .AsNoTracking()
                .Include(i => i.Usuario)
                .Include(i => i.Status)
                .OrderByDescending(i => i.DataRegistro)
                .ToListAsync();

            var resultado = interesses
                .Select(i => new
                {
                    id = i.IdInteresse,
                    nome = i.Usuario != null ? i.Usuario.Nome : "",
                    tipo = TipoParaExibicao(i.Tipo, i.Usuario != null ? i.Usuario.TipoParticipacao : null),
                    descricao = i.Descricao,
                    status = i.Status != null ? i.Status.Descricao : "Pendente",
                    dataSolicitacao = i.DataRegistro.ToString("dd/MM/yyyy")
                })
                .ToList();

            return Ok(resultado);
        }

        [HttpPut("interesses/{id:int}")]
        // Atualiza status, descrição ou tipo da solicitação
        public async Task<IActionResult> EditarInteresse(int id, AtualizarCampoDto dados)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { mensagem = "Dados invalidos." });
            }

            Interesse? interesse = await _context.Interesses.FindAsync(id);

            if (interesse == null)
            {
                return NotFound(new { mensagem = "Interesse nao encontrado." });
            }

            string valor = dados.Valor.Trim();

            // Aplica validação conforme o campo editado
            switch (dados.Campo.Trim().ToLowerInvariant())
            {
                case "status":
                    if (!StatusInteresseOpcoes.TryNormalizar(valor, out string statusNormalizado))
                    {
                        return BadRequest(new { mensagem = "Status invalido. Use Pendente, Concluído ou Cancelado." });
                    }
                    interesse.Status = await ObterOuCriarStatusAsync(statusNormalizado);
                    break;

                case "descricao":
                    if (valor.Length < 5)
                    {
                        return BadRequest(new { mensagem = "Descricao invalida." });
                    }
                    interesse.Descricao = valor;
                    break;

                case "tipo":
                    interesse.Tipo = NormalizarTipo(valor);
                    break;

                default:
                    return BadRequest(new { mensagem = "Campo nao permitido." });
            }

            await _context.SaveChangesAsync();
            return Ok(new { sucesso = true });
        }

        [HttpDelete("interesses/{id:int}")]
        // Exclui interesse cadastrado
        public async Task<IActionResult> ExcluirInteresse(int id)
        {
            Interesse? interesse = await _context.Interesses.FindAsync(id);

            if (interesse == null)
            {
                return NotFound(new { mensagem = "Interesse nao encontrado." });
            }

            _context.Interesses.Remove(interesse);
            await _context.SaveChangesAsync();

            return Ok(new { sucesso = true });
        }

        // Busca ou cria status válido para interesse
        private async Task<StatusInteresse> ObterOuCriarStatusAsync(string descricao)
        {
            if (!StatusInteresseOpcoes.TryNormalizar(descricao, out string statusNormalizado))
            {
                throw new InvalidOperationException("Status invalido.");
            }

            StatusInteresse? status = await _context.StatusInteresses
                .FirstOrDefaultAsync(s => s.Descricao == statusNormalizado);

            if (status != null)
            {
                return status;
            }

            status = new StatusInteresse { Descricao = statusNormalizado };
            _context.StatusInteresses.Add(status);
            return status;
        }

        // Normaliza tipo de participação para salvar no banco
        private static string NormalizarTipo(string tipo)
        {
            return tipo.Trim().ToLowerInvariant() switch
            {
                "doar" => "Doador",
                "doador" => "Doador",
                "receber" => "Receptor",
                "receptor" => "Receptor",
                "ambos" => "Ambos",
                _ => tipo
            };
        }

        // Converte tipo salvo para texto exibido na tabela
        private static string TipoParaExibicao(string? tipo, string? tipoUsuario)
        {
            string valor = string.IsNullOrWhiteSpace(tipo) ? tipoUsuario ?? "" : tipo;

            return valor.Trim().ToLowerInvariant() switch
            {
                "doar" => "Doador",
                "doador" => "Doador",
                "receber" => "Recebedor",
                "recebedor" => "Recebedor",
                "receptor" => "Recebedor",
                "ambos" => "Ambos",
                _ => "Ambos"
            };
        }

        // Remove caracteres não numéricos
        private static string ApenasNumeros(string valor)
        {
            return new string(valor.Where(char.IsDigit).ToArray());
        }
    }
}
