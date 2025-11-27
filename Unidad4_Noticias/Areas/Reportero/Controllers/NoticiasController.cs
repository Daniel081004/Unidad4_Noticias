using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Unidad4_Noticias.Areas.Reportero.Models;
using Unidad4_Noticias.Models.ViewModels;
using Unidad4_Noticias.Services;

namespace Unidad4_Noticias.Areas.Reportero.Controllers
{

    [Area("Reportero")]
    [Authorize(Roles = "Admin, Reportero")]
    public class NoticiasController : Controller
    {
        private readonly NoticiasService noticiasService;
        public NoticiasController(NoticiasService noticiasService)
        {
            this.noticiasService = noticiasService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Vistas = "Noticias";
            int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int mId);
            var vm = noticiasService.GetNoticiasByUsuario(mId);
            return View(vm);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Vistas = "Crear";
            return View();
        }
        [HttpPost]
        public IActionResult Create(ReporteroNoticiasCreateViewModel vm)
        {
            if (vm.Imagen != null)
            {
                if (vm.Imagen.ContentType != "image/jpg" && vm.Imagen.ContentType != "image/png" && vm.Imagen.ContentType != "image/gif" && vm.Imagen.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("", "El formato de imagen no esta permito");
                }
                if(vm.Imagen.Length > 5 * 1024 * 1024)/*5mb*/
                {
                    ModelState.AddModelError("", "El tamaño de la imagen no debe superar los 5MB");
                }
            }
            if (ModelState.IsValid)
            {
                int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int mId);
                int idNoticia = noticiasService.CrearNoticia(vm, mId);
                
                if(vm.Imagen != null && idNoticia != 0)
                {
                    var extension = Path.GetExtension(vm.Imagen.FileName);
                    Directory.CreateDirectory("wwwroot/uploads");
                    FileStream archivoImagen = System.IO.File.Create("wwwroot/uploads/" + idNoticia + extension);
                    vm.Imagen.CopyTo(archivoImagen);
                    archivoImagen.Close();
                }

                return RedirectToAction("Index");
            }
            return View(vm);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View();
        }
    }
}