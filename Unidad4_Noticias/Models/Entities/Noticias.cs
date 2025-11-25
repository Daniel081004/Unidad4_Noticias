using System;
using System.Collections.Generic;

namespace Unidad4_Noticias.Models.Entities;

public partial class Noticias
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Contenido { get; set; } = null!;

    public string? ImagenUrl { get; set; }

    public DateOnly FechaEvento { get; set; }

    public DateTime? FechaPublicacion { get; set; }

    public int UsuarioId { get; set; }

    public bool? Activa { get; set; }

    public virtual ICollection<Comentarios> Comentarios { get; set; } = new List<Comentarios>();

    public virtual Usuarios Usuario { get; set; } = null!;
}
