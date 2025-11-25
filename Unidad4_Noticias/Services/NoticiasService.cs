using Unidad4_Noticias.Models.Entities;
using Unidad4_Noticias.Models.ViewModels;
using Unidad4_Noticias.Areas.Reportero.Models;
using Unidad4_Noticias.Repositories;

namespace Unidad4_Noticias.Services
{
    public class NoticiasService
    {
        private readonly Repository<Noticias> _noticiasRepo;
        private readonly Repository<Comentarios> _comentariosRepo;
        private readonly Repository<Usuarios> _usuariosRepo;

        public NoticiasService(
            Repository<Noticias> noticiasRepo,
            Repository<Comentarios> comentariosRepo,
            Repository<Usuarios> usuariosRepo)
        {
            _noticiasRepo = noticiasRepo;
            _comentariosRepo = comentariosRepo;
            _usuariosRepo = usuariosRepo;
        }

        public IndexViewModel GetNoticiasIndex(string? searchQuery = null)
        {
            var noticias = _noticiasRepo.GetAll()
                .Where(n => n.Activa == true)
                .OrderByDescending(n => n.FechaPublicacion);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                noticias = noticias
                    .Where(n => n.Titulo.Contains(searchQuery) ||
                               n.Contenido.Contains(searchQuery))
                    .OrderByDescending(n => n.FechaPublicacion);
            }

            var noticiasModel = noticias
                .Select(n => new NoticiaModel
                {
                    Id = n.Id,
                    Titulo = n.Titulo,
                    Autor = n.Usuario.Username
                });

            return new IndexViewModel
            {
                SearchQuery = searchQuery,
                Noticias = noticiasModel
            };
        }

        public DetailsViewModel? GetNoticiaDetails(int id)
        {
            var noticia = _noticiasRepo.Get(id);
            if (noticia == null || noticia.Activa == false) return null;

            var comentarios = _comentariosRepo.GetAll()
                .Where(c => c.NoticiaId == id && c.Activo == true)
                .OrderByDescending(c => c.FechaComentario)
                .Select(c => new ComentarioModel
                {
                    Id = c.Id,
                    Autor = c.NombreUsuario,
                    Contenido = c.Contenido,
                    TiempoRelativo = CalcularTiempoRelativo(c.FechaComentario ?? DateTime.Now)
                });

            return new DetailsViewModel
            {
                Id = noticia.Id,
                Titulo = noticia.Titulo,
                Autor = noticia.Usuario.Username,
                FechaPublicacion = noticia.FechaPublicacion ?? DateTime.Now,
                Contenido = noticia.Contenido,
                Comentarios = comentarios
            };
        }

        public bool AgregarComentario(int noticiaId, string contenido, int? usuarioId = null)
        {
            try
            {
                string? nombreUser = null;

                if (usuarioId.HasValue)
                {
                    var usuario = _usuariosRepo.Get(usuarioId.Value);
                    if (usuario != null)
                    {
                        nombreUser = usuario.Username;
                    }
                }

                var comentario = new Comentarios
                {
                    Contenido = contenido,
                    NombreUsuario = nombreUser ?? string.Empty,
                    UsuarioId = usuarioId,
                    NoticiaId = noticiaId,
                    FechaComentario = DateTime.Now,
                    Activo = true,
                    EsReportero = nombreUser != null
                };

                _comentariosRepo.Insert(comentario);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ReporteroNoticiasIndexViewModel GetNoticiasByUsuario(int usuarioId)
        {
            var noticias = _noticiasRepo.GetAll()
                .Where(n => n.UsuarioId == usuarioId)
                .OrderByDescending(n => n.FechaPublicacion)
                .Select(n => new NoticiaModel
                {
                    Id = n.Id,
                    Titulo = n.Titulo
                });

            return new ReporteroNoticiasIndexViewModel
            {
                Noticias = noticias
            };
        }

        public int CrearNoticia(ReporteroNoticiasCreateViewModel model, int usuarioId)
        {
            try
            {
                var noticia = new Noticias
                {
                    Titulo = model.Titulo,
                    Contenido = model.Contenido,
                    FechaEvento = model.FechaEvento.HasValue ?
                        DateOnly.FromDateTime(model.FechaEvento.Value) : DateOnly.FromDateTime(DateTime.Now),
                    FechaPublicacion = DateTime.Now,
                    UsuarioId = usuarioId,
                    Activa = true
                };

                _noticiasRepo.Insert(noticia);
                return noticia.Id;
            }
            catch
            {
                return 0;
            }
        }

        public ReporteroNoticiasEditViewModel? GetNoticiaParaEditar(int id, int usuarioId)
        {
            var noticia = _noticiasRepo.Get(id);
            if (noticia == null || noticia.UsuarioId != usuarioId) return null;

            return new ReporteroNoticiasEditViewModel
            {
                Id = noticia.Id,
                Titulo = noticia.Titulo,
                Contenido = noticia.Contenido,
                FechaEvento = noticia.FechaEvento.ToDateTime(TimeOnly.MinValue)
            };
        }

        public bool EditarNoticia(ReporteroNoticiasEditViewModel model, int usuarioId)
        {
            try
            {
                var noticia = _noticiasRepo.Get(model.Id);
                if (noticia == null || noticia.UsuarioId != usuarioId) return false;

                noticia.Titulo = model.Titulo;
                noticia.Contenido = model.Contenido;
                if (model.FechaEvento.HasValue)
                    noticia.FechaEvento = DateOnly.FromDateTime(model.FechaEvento.Value);

                _noticiasRepo.Update(noticia);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string CalcularTiempoRelativo(DateTime fecha)
        {
            var diferencia = DateTime.Now - fecha;

            if (diferencia.TotalMinutes < 1) return "Hace un momento";
            if (diferencia.TotalMinutes < 60) return $"Hace {(int)diferencia.TotalMinutes} minutos";
            if (diferencia.TotalHours < 24) return $"Hace {(int)diferencia.TotalHours} horas";
            if (diferencia.TotalDays < 30) return $"Hace {(int)diferencia.TotalDays} días";

            return fecha.ToString("dd/MM/yyyy");
        }
    }
}