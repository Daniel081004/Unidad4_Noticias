using System.ComponentModel.DataAnnotations;

namespace Unidad4_Noticias.Areas.Reportero.Models
{
    public class ReporteroNoticiasCreateViewModel
    {
        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200, ErrorMessage = "El título no puede exceder los 200 caracteres")]
        public string Titulo { get; set; } = null!;

        [Display(Name = "Fecha del evento")]
        public DateTime? FechaEvento { get; set; }

        [Display(Name = "Imagen principal")]
        public IFormFile? Imagen { get; set; }

        [Required(ErrorMessage = "El contenido es obligatorio")]
        [MinLength(10, ErrorMessage = "El contenido debe tener al menos 10 caracteres")]
        public string Contenido { get; set; } = null!;
    }
}
