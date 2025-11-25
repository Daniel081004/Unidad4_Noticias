using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Unidad4_Noticias.Models.ViewModels;
using Unidad4_Noticias.Services;

namespace Unidad4_Noticias.Controllers
{
    public class AccountController : Controller
    {
        private readonly UsuariosService usuariosService;
        public AccountController(UsuariosService usuariosService)
        {
            this.usuariosService = usuariosService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = usuariosService.Autenticar(model.Username ?? "", model.Password ?? "");
                if(usuario == null)
                {
                    ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                    return View(model);
                }
                List<Claim> claims = [];
                claims.Add(new Claim(ClaimTypes.Name, usuario.Username));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()));
                claims.Add(new Claim(ClaimTypes.Role, usuario.Rol.Nombre));
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(principal, new AuthenticationProperties
                {
                    IsPersistent = true
                });
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}