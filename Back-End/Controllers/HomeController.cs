using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    // Controller responsável pelas páginas principais e cadastro público
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        // Salva usuário e interesse enviados pelo cadastro público
        public async Task<IActionResult> Cadastro([FromBody] CadastroViewModel cadastro)
        {
            if (cadastro == null)
            {
                return BadRequest(new { sucesso = false, mensagem = "Dados do cadastro nao foram enviados." });
            }

            string cpf = ApenasNumeros(cadastro.Cpf);
            string telefone = ApenasNumeros(cadastro.Telefone);
            string nome = cadastro.Nome?.Trim() ?? "";
            string? email = string.IsNullOrWhiteSpace(cadastro.Email)
                ? null
                : cadastro.Email.Trim();
            string endereco = string.IsNullOrWhiteSpace(cadastro.Endereco)
                ? "Endereco nao informado"
                : cadastro.Endereco.Trim();
            string cidade = string.IsNullOrWhiteSpace(cadastro.Cidade)
                ? "Nao informada"
                : cadastro.Cidade.Trim();
            string estado = string.IsNullOrWhiteSpace(cadastro.Estado)
                ? "NI"
                : cadastro.Estado.Trim().ToUpperInvariant();
            string necessidade = cadastro.Necessidade?.Trim() ?? "";

            // Valida campos obrigatórios enviados pela tela atual
            if (nome.Length < 3)
            {
                return BadRequest(new { sucesso = false, mensagem = "Informe o nome completo." });
            }

            if (!string.IsNullOrWhiteSpace(email) &&
                !new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(email))
            {
                return BadRequest(new { sucesso = false, mensagem = "Informe um email valido." });
            }

            if (telefone.Length is < 10 or > 11)
            {
                return BadRequest(new { sucesso = false, mensagem = "Informe um telefone valido." });
            }

            if (string.IsNullOrWhiteSpace(necessidade))
            {
                return BadRequest(new { sucesso = false, mensagem = "Informe a necessidade." });
            }

            if (estado.Length != 2)
            {
                estado = "NI";
            }

            if (!CpfValido(cpf))
            {
                return BadRequest(new { sucesso = false, mensagem = "CPF invalido." });
            }

            if (await _context.Usuarios.AnyAsync(u => u.Cpf == cpf))
            {
                return Conflict(new { sucesso = false, mensagem = "CPF ja cadastrado." });
            }

            // Garante UF e status padrão antes de salvar o cadastro
            Uf uf = await ObterOuCriarUfAsync(estado);
            StatusInteresse status = await ObterOuCriarStatusAsync(StatusInteresseOpcoes.Pendente);

            var usuario = new Usuario
            {
                Nome = nome,
                Email = email,
                Telefone = telefone,
                Cpf = cpf,
                Endereco = endereco,
                Cidade = cidade,
                DataNascimento = cadastro.DataNascimento?.Date ?? new DateTime(1900, 1, 1),
                DataCadastro = DateTime.Now,
                TipoParticipacao = NormalizarTipo(cadastro.TipoParticipacao),
                Uf = uf
            };

            var interesse = new Interesse
            {
                Usuario = usuario,
                Status = status,
                Tipo = usuario.TipoParticipacao,
                Descricao = necessidade,
                DataRegistro = DateTime.Now
            };

            // Salva novo usuário e interesse no banco de dados
            _context.Usuarios.Add(usuario);
            _context.Interesses.Add(interesse);
            await _context.SaveChangesAsync();

            return Ok(new { sucesso = true });
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Admin()
        {
            return View();
        }

        [Authorize(Roles = "Atendente")]
        public IActionResult Atendente()
        {
            return View();
        }

        public IActionResult NossaHistoria()
        {
            return View();
        }

        public IActionResult LoginAdmin()
        {
            return View();
        }

        [HttpPost]
        // Valida login tradicional por e-mail e senha
        public async Task<IActionResult> LoginAdmin(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return Unauthorized("Informe email e senha validos.");
            }

            string email = login.Email.Trim().ToLowerInvariant();

            Admin? admin = await _context.Administradores
                .FirstOrDefaultAsync(a => a.Email != null && a.Email.ToLower() == email);

            if (admin != null && admin.VerificarSenha(login.Senha))
            {
                await EntrarAsync(admin.IdAdmin.ToString(), admin.Email ?? "", "Administrador");
                return RedirectToAction(nameof(Admin));
            }

            Atendente? atendente = await _context.Atendentes
                .FirstOrDefaultAsync(a => a.Email != null && a.Email.ToLower() == email);

            if (atendente != null && atendente.VerificarSenha(login.Senha))
            {
                await EntrarAsync(atendente.IdAtendente.ToString(), atendente.Email ?? "", "Atendente");
                return RedirectToAction(nameof(Atendente));
            }

            return Unauthorized("Email ou senha invalidos.");
        }

        public IActionResult Doacao()
        {
            return View();
        }

        [HttpPost]
        // Encerra sessão autenticada
        public async Task<IActionResult> Sair()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Index));
        }

        // Cria cookie de autenticação com perfil de acesso
        private async Task EntrarAsync(string id, string email, string tipo)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, tipo)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                });
        }

        // Busca ou cria UF para vincular ao usuário
        private async Task<Uf> ObterOuCriarUfAsync(string sigla)
        {
            Uf? uf = await _context.Ufs.FirstOrDefaultAsync(u => u.Sigla == sigla);

            if (uf != null)
            {
                return uf;
            }

            uf = new Uf
            {
                Sigla = sigla,
                Nome = sigla
            };

            _context.Ufs.Add(uf);
            return uf;
        }

        // Busca ou cria status válido para o interesse
        private async Task<StatusInteresse> ObterOuCriarStatusAsync(string descricao)
        {
            if (!StatusInteresseOpcoes.TryNormalizar(descricao, out string statusNormalizado))
            {
                throw new InvalidOperationException("Status invalido.");
            }

            StatusInteresse? status = await _context.StatusInteresses.FirstOrDefaultAsync(s => s.Descricao == statusNormalizado);

            if (status != null)
            {
                return status;
            }

            status = new StatusInteresse
            {
                Descricao = statusNormalizado
            };

            _context.StatusInteresses.Add(status);
            return status;
        }

        // Normaliza tipo de participação recebido do formulário
        private static string NormalizarTipo(string? tipo)
        {
            return (tipo ?? "").Trim().ToLowerInvariant() switch
            {
                "doar" => "Doador",
                "receber" => "Receptor",
                "ambos" => "Ambos",
                "doador" => "Doador",
                "receptor" => "Receptor",
                _ => "Ambos"
            };
        }

        // Remove caracteres não numéricos
        private static string ApenasNumeros(string valor)
        {
            return new string(valor.Where(char.IsDigit).ToArray());
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


        public IActionResult Privacy()
        {
            return RedirectToAction(nameof(Cadastro));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
