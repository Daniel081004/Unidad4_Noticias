using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unidad4_Noticias.Models.ViewModels;
using Unidad4_Noticias.Services;

namespace Unidad4_Noticias.Controllers
{
    public class HomeController : Controller
    {
        private readonly NoticiasService noticiasService;
        public HomeController(NoticiasService noticiasService)
        {
            this.noticiasService = noticiasService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Vistas = "Inicio";
            var noticias = noticiasService.GetNoticiasIndex();
            return View(noticias);
        }
        [HttpPost]
        public IActionResult Index(IndexViewModel vm)
        {
            var noticias = noticiasService.GetNoticiasIndex(vm.SearchQuery);
            return View(noticias);
        }
        public IActionResult Details(string id)/*Nombre*/
        {
            var vm = noticiasService.GetNoticiaDetails(id);
            return View(vm);
        }
    }
}