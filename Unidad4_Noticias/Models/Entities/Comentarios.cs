using System;
using System.Collections.Generic;

namespace Unidad4_Noticias.Models.Entities;

public partial class Comentarios
{
    public int Id { get; set; }

    public string Contenido { get; set; } = null!;

    public string NombreUsuario { get; set; } = null!;

    public bool? EsReportero { get; set; }

    public int? UsuarioId { get; set; }

    public int NoticiaId { get; set; }

    public DateTime? FechaComentario { get; set; }

    public bool? Activo { get; set; }

    public virtual Noticias Noticia { get; set; } = null!;

    public virtual Usuarios? Usuario { get; set; }
}
