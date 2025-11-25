namespace Unidad4_Noticias.Models.ViewModels
{
    public class IndexViewModel
    {
        public string? SearchQuery { get; set; }
        public IEnumerable<NoticiaModel>? Noticias { get; set; }
    }

    public class NoticiaModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
    }
}
