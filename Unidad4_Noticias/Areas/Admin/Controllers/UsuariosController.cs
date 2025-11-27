using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Unidad4_Noticias.Areas.Admin.Models;
using Unidad4_Noticias.Models.ViewModels;
using Unidad4_Noticias.Services;

namespace Unidad4_Noticias.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsuariosController : Controller
    {
        private readonly UsuariosService usuariosService;

        public UsuariosController(UsuariosService usuariosService)
        {
            this.usuariosService = usuariosService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Vistas = "Usuarios";
            var vm = usuariosService.GetUsuariosAdmin();
            return View(vm);
        }
        [HttpGet]
        public IActionResult Crear()
        {
            var vm = usuariosService.GetUsuariosAdmin();
            vm.NuevoUsuario = new(); /*Para abrir el modal de crear usuario*/
            return View("Index",vm);
        }
        [HttpPost]
        public IActionResult Crear(AdminUsuariosIndexViewModel vm)
        {
            if(ModelState.IsValid && vm.NuevoUsuario != null)
            {
                usuariosService.CrearUsuario(vm.NuevoUsuario);
                return RedirectToAction("Index");
            }
            else
            {
                //Para mostrar el validationSumary
                var model = usuariosService.GetUsuariosAdmin();
                model.NuevoUsuario = vm.NuevoUsuario;
                return RedirectToAction("Index",model);
            }       
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var vm = usuariosService.GetUsuariosAdmin();
            vm.UsuarioEditar = usuariosService.GetUsuarioParaEditar(id); /*Para abrir el modal de crear usuario*/
            return View("Index", vm);
        }
        [HttpPost]
        public IActionResult Editar(AdminUsuariosIndexViewModel vm)
        {
            if (ModelState.IsValid && vm.UsuarioEditar != null)
            {
                usuariosService.EditarUsuario(vm.UsuarioEditar);
                return RedirectToAction("Index");
            }
            else
            {
                //Para mostrar el validationSumary
                var model = usuariosService.GetUsuariosAdmin();
                model.UsuarioEditar = vm.UsuarioEditar;
                return RedirectToAction("Index",model);
            }
        }

        [HttpGet]
        public IActionResult CambiarPassword(int id)
        {
            var vm = usuariosService.GetUsuariosAdmin();
            vm.CambioPassword = new() { UserId = id }; /*Para abrir el modal de crear usuario*/
            return View("Index", vm);
        }
        [HttpPost]
        public IActionResult CambiarPassword(AdminUsuariosIndexViewModel vm)
        {
            if (!ModelState.IsValid && vm.CambioPassword != null)
            {
                usuariosService.CambiarPassword(vm.CambioPassword);
                return RedirectToAction("Index");
            }
            else
            {
                //Para mostrar el validationSumary
                var model = usuariosService.GetUsuariosAdmin();
                model.CambioPassword = vm.CambioPassword;
                return RedirectToAction("Index",model);
            }
        }

        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var vm = usuariosService.GetUsuariosAdmin();
            vm.UsuarioEliminar = usuariosService.GetUsuarioParaEliminar(id); /*Para abrir el modal de crear usuario*/
            return View("Index", vm);
        }
        [HttpPost]
        public IActionResult Eliminar(AdminUsuariosIndexViewModel vm)
        {
            if (vm.UsuarioEliminar != null)
            {
                int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,out int mId);
                if(vm.UsuarioEliminar.UserId != mId)
                {
                    usuariosService.EliminarUsuario(vm.UsuarioEliminar.UserId);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No se puede eliminar el usuario indicado.");
                }
                
            }
            //Para mostrar el validationSumary
            var model = usuariosService.GetUsuariosAdmin();
            model.UsuarioEliminar = vm.UsuarioEliminar;
            return RedirectToAction("Index", model);
        }
    }
}