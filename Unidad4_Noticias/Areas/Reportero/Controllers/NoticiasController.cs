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
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ReporteroNoticiasCreateViewModel vm)
        {
            if(!ModelState.IsValid)
            {
                int.TryParse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int mId);
                int idNoticia = noticiasService.CrearNoticia(vm, mId);
                if(vm.Imagen != null && idNoticia != 0)
                {
                    /**/
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