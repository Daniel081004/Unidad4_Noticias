using System.ComponentModel.DataAnnotations;

namespace Unidad4_Noticias.Models.ViewModels
{
    public class DetailsViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Autor { get; set; } = null!;
        public DateTime FechaPublicacion { get; set; }
        public string Contenido { get; set; } = null!;
        public IEnumerable<ComentarioModel>? Comentarios { get; set; }
        public NuevoComentarioModel? NuevoComentario { get; set; }
    }

    public class ComentarioModel
    {
        public int Id { get; set; }
        public string Autor { get; set; } = null!;
        public string TiempoRelativo { get; set; } = null!; // "Hace 2 horas", "Hace 1 día"
        public string Contenido { get; set; } = null!;
    }

    public class NuevoComentarioModel
    {
        [Required(ErrorMessage = "El comentario es requerido")]
        [StringLength(300, ErrorMessage = "El comentario no puede exceder los 300 caracteres")]
        public string Contenido { get; set; } = null!;
    }
}
