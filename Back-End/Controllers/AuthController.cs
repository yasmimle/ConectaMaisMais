using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/auth")]
    // API responsável pela autenticação de Admin e Atendente
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        // Realiza login com e-mail, senha e tipo de usuário
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { sucesso = false, mensagem = "Informe email e senha validos." });
            }

            string email = login.Email.Trim().ToLowerInvariant();

            // Verifica credenciais de administrador
            Admin? admin = await _context.Administradores
                .FirstOrDefaultAsync(a => a.Email != null && a.Email.ToLower() == email);

            if (admin != null && admin.VerificarSenha(login.Senha))
            {
                await EntrarAsync(admin.IdAdmin.ToString(), admin.Email ?? "", "Administrador");
                return Ok(new { sucesso = true, tipo = "Administrador", destino = Url.Action("Admin", "Home") });
            }

            // Verifica credenciais de atendente
            Atendente? atendente = await _context.Atendentes
                .FirstOrDefaultAsync(a => a.Email != null && a.Email.ToLower() == email);

            if (atendente != null && atendente.VerificarSenha(login.Senha))
            {
                await EntrarAsync(atendente.IdAtendente.ToString(), atendente.Email ?? "", "Atendente");
                return Ok(new { sucesso = true, tipo = "Atendente", destino = Url.Action("Atendente", "Home") });
            }

            return Unauthorized(new { sucesso = false, mensagem = "Email ou senha invalidos." });
        }

        [HttpPost("sair")]
        // Encerra sessão autenticada
        public async Task<IActionResult> Sair()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { sucesso = true });
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
    }
}
