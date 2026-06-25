using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Authorize(Roles = "Administrador")]
    [Route("api/admin")]
    // API responsável pelos CRUDs do painel administrativo
    public class AdminApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminApiController(ApplicationDbContext context)
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

        [HttpDelete("usuarios/{id:int}")]
        // Remove usuário e seus interesses vinculados
        public async Task<IActionResult> ExcluirUsuario(int id)
        {
            Usuario? usuario = await _context.Usuarios
                .Include(u => u.Interesses)
                .FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound(new { mensagem = "Usuario nao encontrado." });
            }

            _context.Usuarios.Remove(usuario);
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

        [HttpGet("administradores")]
        // Lista administradores cadastrados
        public async Task<IActionResult> ListarAdministradores()
        {
            var administradores = await _context.Administradores
                .AsNoTracking()
                .OrderBy(a => a.Nome)
                .Select(a => new
                {
                    id = a.IdAdmin,
                    nome = a.Nome,
                    email = a.Email
                })
                .ToListAsync();

            return Ok(administradores);
        }

        [HttpGet("atendentes")]
        // Lista atendentes cadastrados
        public async Task<IActionResult> ListarAtendentes()
        {
            var atendentes = await _context.Atendentes
                .AsNoTracking()
                .OrderBy(a => a.Nome)
                .Select(a => new
                {
                    id = a.IdAtendente,
                    nome = a.Nome,
                    email = a.Email,
                    status = "Ativo"
                })
                .ToListAsync();

            return Ok(atendentes);
        }

        [HttpPost("administradores")]
        // Cria novo administrador com senha criptografada
        public async Task<IActionResult> CriarAdministrador(CriarAcessoDto dados)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { mensagem = "Dados invalidos." });
            }

            if (await _context.Administradores.AnyAsync(a => a.Email == dados.Email))
            {
                return Conflict(new { mensagem = "Email de administrador ja cadastrado." });
            }

            Ong ong = await ObterOuCriarOngAsync();
            var admin = new Admin
            {
                Nome = dados.Nome.Trim(),
                Email = dados.Email.Trim(),
                Ong = ong
            };

            admin.DefinirSenha(dados.Senha);
            _context.Administradores.Add(admin);
            await _context.SaveChangesAsync();

            return Ok(new { sucesso = true });
        }

        [HttpPost("atendentes")]
        // Cria novo atendente com senha criptografada
        public async Task<IActionResult> CriarAtendente(CriarAcessoDto dados)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { mensagem = "Dados invalidos." });
            }

            if (await _context.Atendentes.AnyAsync(a => a.Email == dados.Email))
            {
                return Conflict(new { mensagem = "Email de atendente ja cadastrado." });
            }

            Ong ong = await ObterOuCriarOngAsync();
            var atendente = new Atendente
            {
                Nome = dados.Nome.Trim(),
                Email = dados.Email.Trim(),
                Ong = ong
            };

            atendente.DefinirSenha(dados.Senha);
            _context.Atendentes.Add(atendente);
            await _context.SaveChangesAsync();

            return Ok(new { sucesso = true });
        }

        [HttpPut("administradores/{id:int}")]
        // Atualiza dados de acesso do administrador
        public async Task<IActionResult> EditarAdministrador(int id, AtualizarCampoDto dados)
        {
            Admin? admin = await _context.Administradores.FindAsync(id);

            if (admin == null)
            {
                return NotFound(new { mensagem = "Administrador nao encontrado." });
            }

            if (dados.Campo.Trim().Equals("email", StringComparison.OrdinalIgnoreCase) &&
                await _context.Administradores.AnyAsync(a => a.IdAdmin != id && a.Email == dados.Valor.Trim()))
            {
                return Conflict(new { mensagem = "Email de administrador ja cadastrado." });
            }

            IActionResult? resultado = AtualizarAcesso(admin, dados);
            if (resultado != null)
            {
                return resultado;
            }

            await _context.SaveChangesAsync();
            return Ok(new { sucesso = true });
        }

        [HttpPut("atendentes/{id:int}")]
        // Atualiza dados de acesso do atendente
        public async Task<IActionResult> EditarAtendente(int id, AtualizarCampoDto dados)
        {
            Atendente? atendente = await _context.Atendentes.FindAsync(id);

            if (atendente == null)
            {
                return NotFound(new { mensagem = "Atendente nao encontrado." });
            }

            if (dados.Campo.Trim().Equals("email", StringComparison.OrdinalIgnoreCase) &&
                await _context.Atendentes.AnyAsync(a => a.IdAtendente != id && a.Email == dados.Valor.Trim()))
            {
                return Conflict(new { mensagem = "Email de atendente ja cadastrado." });
            }

            IActionResult? resultado = AtualizarAcesso(atendente, dados);
            if (resultado != null)
            {
                return resultado;
            }

            await _context.SaveChangesAsync();
            return Ok(new { sucesso = true });
        }

        [HttpDelete("administradores/{id:int}")]
        // Remove administrador do sistema
        public async Task<IActionResult> ExcluirAdministrador(int id)
        {
            Admin? admin = await _context.Administradores.FindAsync(id);

            if (admin == null)
            {
                return NotFound(new { mensagem = "Administrador nao encontrado." });
            }

            _context.Administradores.Remove(admin);
            await _context.SaveChangesAsync();

            return Ok(new { sucesso = true });
        }

        [HttpDelete("atendentes/{id:int}")]
        // Remove atendente do sistema
        public async Task<IActionResult> ExcluirAtendente(int id)
        {
            Atendente? atendente = await _context.Atendentes.FindAsync(id);

            if (atendente == null)
            {
                return NotFound(new { mensagem = "Atendente nao encontrado." });
            }

            _context.Atendentes.Remove(atendente);
            await _context.SaveChangesAsync();

            return Ok(new { sucesso = true });
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

        // Busca a ONG padrão ou cria registro inicial
        private async Task<Ong> ObterOuCriarOngAsync()
        {
            Ong? ong = await _context.Ongs.FirstOrDefaultAsync();

            if (ong != null)
            {
                return ong;
            }

            ong = new Ong
            {
                Nome = "Conecta++",
                Cnpj = "12345678000199",
                Telefone = "11919100860",
                Email = "contato@ong.com",
                Endereco = "Rua Gugu, Sao Paulo SP"
            };

            _context.Ongs.Add(ong);
            return ong;
        }

        // Valida e atualiza dados de login de Admin ou Atendente
        private IActionResult? AtualizarAcesso(UsuarioSistema pessoa, AtualizarCampoDto dados)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { mensagem = "Dados invalidos." });
            }

            string valor = dados.Valor.Trim();

            switch (dados.Campo.Trim().ToLowerInvariant())
            {
                case "email":
                    if (!new EmailAddressAttribute().IsValid(valor))
                    {
                        return BadRequest(new { mensagem = "Email invalido." });
                    }
                    pessoa.Email = valor;
                    return null;

                case "senha":
                    if (valor.Length < 6)
                    {
                        return BadRequest(new { mensagem = "A senha deve ter pelo menos 6 caracteres." });
                    }
                    pessoa.DefinirSenha(valor);
                    return null;

                default:
                    return BadRequest(new { mensagem = "Campo nao permitido." });
            }
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
