using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TareasMVC.Models;

namespace TareasMVC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public UsuariosController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }


        [AllowAnonymous]
        public IActionResult Registro() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
            if (!ModelState.IsValid) return View(modelo);

            var usuario = new IdentityUser() { Email = modelo.Email, UserName = modelo.Email };
            var resultado = await userManager.CreateAsync(usuario, password: modelo.Password);
            if (!resultado.Succeeded)
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            };
            await signInManager.SignInAsync(usuario, isPersistent: true);
            return RedirectToAction("Index", "Home");
        }




        [AllowAnonymous]
        public IActionResult Login(String mensaje = null)
        {
            if (mensaje is not null) ViewData["Mensaje"] = mensaje;
            
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel modelo)
        {
            if (!ModelState.IsValid) return View(modelo);

            var resultado = await signInManager.PasswordSignInAsync(modelo.Email, modelo.Password, modelo.Recuerdame, lockoutOnFailure: false);

            if (!resultado.Succeeded)
            {

                ModelState.AddModelError(string.Empty, "Nombre de usurio/password incorrecto");
            };

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [AllowAnonymous]
        public ChallengeResult LoginExterno(string proveedor, string returnUrl = null)
        {
            var urlRedireccion = Url.Action("RegistrarUsuarioExterno", values: new { returnUrl });
            var propiedades = signInManager.ConfigureExternalAuthenticationProperties(proveedor, urlRedireccion);
            return new ChallengeResult(proveedor, propiedades);
        }


        [AllowAnonymous]
        public async Task<IActionResult> RegistrarUsuarioExterno(string urlRetorno = null, string remoteError = null)
        {
            urlRetorno = urlRetorno ?? Url.Content("~/");
            var mensaje = "";
            if (remoteError is not null)
            {
                mensaje = $"Error del proveedor externo: {remoteError}";
                return RedirectToAction("Login", routeValues: new { mensaje });
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info is null)
            {
                mensaje = $"Error al cargar la información del proveedor externo durante el inicio de sesión";
                return RedirectToAction("Login", routeValues: new { mensaje });
            }

            var resultado = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true, bypassTwoFactor: true);

            // La cuenta del usuario ya existe
            if (resultado.Succeeded)
            {
                return LocalRedirect(urlRetorno);
            }

            // La cuenta del usuario no existe
            var email = "";

            if (!info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                mensaje = "El proveedor de inicio de sesión no proporcionó un correo electrónico";
                return RedirectToAction("Login", routeValues: new { mensaje });
            }

            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                email = info.Principal.FindFirstValue(ClaimTypes.Email);
            }

            var usaurio = new IdentityUser() { Email = email, UserName = email };
            var resultadoCreacion = await userManager.CreateAsync(usaurio);
            if (!resultadoCreacion.Succeeded)
            {
                mensaje = "Error al crear la cuenta del usuario";
                return RedirectToAction("Login", routeValues: new { mensaje });
            }

            var resultadoLogin = await userManager.AddLoginAsync(usaurio, info);
            if (!resultadoLogin.Succeeded)
            {
                mensaje = "Error al agregar el inicio de sesión externo a la cuenta del usuario";
                return RedirectToAction("Login", routeValues: new { mensaje });
            }

            await signInManager.SignInAsync(usaurio, isPersistent: true, info.LoginProvider);
            return LocalRedirect(urlRetorno);
        }




    }
}
